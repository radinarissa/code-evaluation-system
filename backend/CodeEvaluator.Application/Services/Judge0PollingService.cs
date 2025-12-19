using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using CodeEvaluator.Infrastructure.Data;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Application.DTOs;
using System.Net.Http;

namespace CodeEvaluator.Application.Services
{
    public class Judge0PollingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Judge0PollingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var judge0Service = scope.ServiceProvider.GetRequiredService<IJudge0Service>();

                Console.WriteLine("Polling Judge0 for pending results: ");
                var pendingResults = await db.TestResults
                    .Include(r => r.Submission)
                    .Where(r => r.Status == "Pending" && r.Judge0Token != null)
                    .ToListAsync(stoppingToken);
                Console.WriteLine("Found: " + pendingResults.Count);

                if (pendingResults.Any())
                {
                    var pendingByToken = pendingResults.ToDictionary(r => r.Judge0Token!);
                    var tokens = pendingByToken.Keys.ToList();
                    var results = await judge0Service.GetSubbmisionBatchAsync(tokens);
                    Console.WriteLine("found pending results: " + results.Count);

                    foreach (var res in results)
                    {
                        if (res == null)
                            continue;
                        if (res.Status.Id < 3)
                            continue;
                        if (!pendingByToken.TryGetValue(res.Token, out var testResult))
                            continue;

                        await db.Entry(testResult).Reference(tr => tr.TestCase).LoadAsync();

                        testResult.Status = DetermineTestStatus(res, testResult.TestCase.ExpectedOutput);

                        if (res.Time != null)
                        {
                            testResult.ExecutionTime = float.Parse(res.Time);
                        }
                        testResult.MemoryUsage = res.Memory;
                        testResult.DiskUsedMb = res.FileSize / 1024m;
                        testResult.Output = res.Stdout;
                        testResult.ErrorMessage = res.Stderr;
                        testResult.Judge0Token = res.Token;

                        Console.WriteLine("updated test result for token: " + res.Token);

                        var allresultsforsubmission = db.TestResults.Where(sb => sb.SubmissionId == testResult.Submission.Id);
                        bool alldone = allresultsforsubmission.All(ares => ares.Status != "Pending");

                        if (alldone)
                        {
                            await db.Entry(testResult.Submission).Reference(s => s.Task).LoadAsync();

                            var allTestResults = await db.TestResults
                                .Include(tr => tr.TestCase)
                                .Where(tr => tr.SubmissionId == testResult.Submission.Id)
                                .ToListAsync(stoppingToken);

                            int totalTests = allTestResults.Count;
                            int passedTests = allTestResults.Count(tr => tr.Status == "Pass");
                            decimal maxPoints = testResult.Submission.Task.MaxPoints;
                            decimal finalGrade = totalTests > 0 ? (decimal)passedTests / totalTests * maxPoints : 0;

                            string feedback = GenerateFeedback(allTestResults, passedTests, totalTests);

                            testResult.Submission.Status = "Completed";
                            testResult.Submission.FinalGrade = finalGrade;
                            testResult.Submission.Feedback = feedback;

                            Console.WriteLine($"Submission {testResult.Submission.Id} completed: {finalGrade}/{maxPoints} points");

                            if (testResult.Submission.MoodleSubmissionId.HasValue)
                            {
                                var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
                                await SyncGradeToMoodle(httpClient, testResult.Submission, finalGrade, feedback);
                            }
                        }
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
                await System.Threading.Tasks.Task.Delay(20000, stoppingToken);
            }
        }

        private string DetermineTestStatus(Judge0ResultDto judge0Result, string expectedOutput)
        {
            if (judge0Result.Status.Id == 6)
            {
                return "Compilation Error";
            }
            else if (judge0Result.Status.Id == 5 || judge0Result.Status.Id == 11 || judge0Result.Status.Id == 12)
            {
                return "Runtime Error";
            }
            else if (judge0Result.Status.Id == 4)
            {
                return "Timeout";
            }
            else if (judge0Result.Status.Id == 7)
            {
                return "Memory Limit";
            }
            else if (judge0Result.Status.Id == 3)
            {
                string actualOutput = judge0Result.Stdout?.Trim() ?? "";
                string expected = expectedOutput?.Trim() ?? "";

                if (actualOutput == expected)
                {
                    return "Pass";
                }
                else
                {
                    return "Fail";
                }
            }
            else
            {
                return "Unknown";
            }
        }

        private string GenerateFeedback(List<TestResult> testResults, int passedTests, int totalTests)
        {
            if (passedTests == totalTests)
            {
                return "Поздравления! Всички тестове са преминати успешно!";
            }

            var feedbackLines = new List<string>();
            feedbackLines.Add($"Успешно преминати тестове: {passedTests} от {totalTests}");

            foreach (var testResult in testResults.Where(tr => tr.Status != "Pass"))
            {
                string testName = testResult.TestCase.Name;
                string message = testResult.Status switch
                {
                    "Compilation Error" => $"Компилационна грешка: {testResult.ErrorMessage}",
                    "Timeout" => $"Програмата не преминава тестът \"{testName}\" за достатъчно кратко време.",
                    "Memory Limit" => $"Програмата не преминава тестът \"{testName}\" поради изчерпване на паметта. Използвана памет: {testResult.MemoryUsage} KB.",
                    "Disk Limit" => $"Програмата не преминава тестът \"{testName}\" поради изчерпване на позволеното дисково пространство. Използвано: {testResult.DiskUsedMb} MB.",
                    "Runtime Error" => $"Runtime грешка в тест \"{testName}\": {testResult.ErrorMessage}",
                    "Fail" => $"Неуспешно изпълнение на тест \"{testName}\" - резултатът се различава от очакваното.",
                    _ => $"Грешка в тест \"{testName}\": {testResult.Status}"
                };
                feedbackLines.Add(message);
            }

            return string.Join("\n", feedbackLines);
        }

        private async System.Threading.Tasks.Task SyncGradeToMoodle(HttpClient httpClient, Submission submission, decimal grade, string feedback)
        {
            try
            {
                var moodleUrl = "http://localhost:8000";
                var wsToken = "bf739980f1493b05364cb573ce2fb009"; //moodle token

                var url = $"{moodleUrl}/webservice/rest/server.php" +
                          $"?wstoken={wsToken}" +
                          $"&wsfunction=mod_assign_save_grade" +
                          $"&moodlewsrestformat=json" +
                          $"&assignmentid={submission.Task.MoodleAssignmentId}" +
                          $"&userid={submission.UserId}" +
                          $"&grade={grade}" +
                          $"&attemptnumber={submission.AttemptNumber}" +
                          $"&addattempt=0" +
                          $"&workflowstate=" +
                          $"&applytoall=0";

                var feedbackUrl = url + $"&plugindata[assignfeedbackcomments_editor][text]={Uri.EscapeDataString(feedback)}" +
                                       $"&plugindata[assignfeedbackcomments_editor][format]=1";

                var response = await httpClient.PostAsync(feedbackUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    submission.MoodleSyncStatus = "Synced";
                    submission.MoodleSyncCreatedAt = DateTime.UtcNow;
                    Console.WriteLine($"Moodle sync successful for submission {submission.Id}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    submission.MoodleSyncStatus = "Failed";
                    submission.MoodleSyncOutput = errorContent;
                    Console.WriteLine($"Moodle sync failed for submission {submission.Id}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                submission.MoodleSyncStatus = "Error";
                submission.MoodleSyncOutput = ex.Message;
                Console.WriteLine($"Moodle sync error for submission {submission.Id}: {ex.Message}");
            }
        }
    }
}