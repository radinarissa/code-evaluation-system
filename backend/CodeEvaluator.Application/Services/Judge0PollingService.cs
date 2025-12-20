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

namespace CodeEvaluator.Application.Services{
    

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
            var all = await db.TestResults.ToListAsync(stoppingToken);
            Console.WriteLine("Total test results in DB: " + all.Count);
            

            if (pendingResults.Any())
            {
                var pendingByToken = pendingResults.ToDictionary(r=> r.Judge0Token!);
                var tokens = pendingByToken.Keys.ToList();
                var results = await judge0Service.GetSubbmisionBatchAsync(tokens);
                Console.WriteLine("should be "+pendingResults.Count());
                Console.WriteLine("found pending results: " + results.Count);
                // foreach(var res in results)
                // {
                //     if(res==null) 
                //         continue;
                //     if(res.Status.Id<3) // In Queue or Processing
                //         continue;
                //     if(!pendingByToken.TryGetValue(res.Token, out var testResult))
                //         continue;

                //             testResult.Status = res.Status.Description;
                //         if (res.Time != null)
                //         {
                //              testResult.ExecutionTime = float.Parse(res.Time);
                //         }
                //             testResult.MemoryUsage = res.Memory;
                //             testResult.DiskUsedMb = res.FileSize/1024m;
                //             testResult.Output = res.Stdout;
                //             testResult.ErrorMessage = res.Stderr;
                //             testResult.Judge0Token = res.Token;
                           
                //            Console.WriteLine("updated test result for token: " + res.Token);
                //            Console.WriteLine("Stdout: " + res.Stdout);


                //         // TestResultCs also keeps instances of the TestCase and The Submission?

                //         var allresultsforsubmission = db.TestResults.Where(sb=>sb.SubmissionId == testResult.Submission.Id);
                //        bool alldone = allresultsforsubmission.All(ares => ares.Status != "Pending");
                     
                     
                //         if(alldone){
                //             testResult.Submission.Status="Completed";
                         
                //         }
                // }
                foreach (var res in results)
                {
                    if (res == null)
                        continue;

                    // In Queue / Processing => skip until finished
                    if (res.Status != null && res.Status.Id < 3)
                        continue;

                    if (string.IsNullOrWhiteSpace(res.Token))
                        continue;

                    if (!pendingByToken.TryGetValue(res.Token, out var testResult))
                        continue;

                    // 1) Status
                    testResult.Status = res.Status?.Description ?? "Unknown";

                    if (!string.IsNullOrWhiteSpace(res.Time) &&
                        float.TryParse(res.Time,
                            System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out var execTime))
                    {
                        testResult.ExecutionTime = execTime;
                    }

                    testResult.MemoryUsage = res.Memory;

                    // DiskUsedMb: store MB from filesize (bytes)
                    if (res.FileSize.HasValue)
                    {
                        testResult.DiskUsedMb = (decimal)res.FileSize.Value / 1024m / 1024m;
                    }

                    testResult.Output = res.Stdout ?? "";

                    // IMPORTANT: compile errors are in CompileOutput, not stderr
                    testResult.ErrorMessage =
                        !string.IsNullOrWhiteSpace(res.CompileOutput) ? res.CompileOutput :
                        !string.IsNullOrWhiteSpace(res.Stderr) ? res.Stderr :
                        !string.IsNullOrWhiteSpace(res.Message) ? res.Message :
                        "";

                    //testResult.ErrorMessage = err;

                    // Keep token (not strictly necessary)
                    testResult.Judge0Token = res.Token;

                    Console.WriteLine("updated test result for token: " + res.Token);
                    Console.WriteLine("Stdout: " + (res.Stdout ?? ""));

                    // IMPORTANT: use SubmissionId, not testResult.Submission.Id
                    // (testResult.Submission can be null if Include didn't load or tracking differs)
                    var submissionId = testResult.SubmissionId;

                    // Make this query run in DB (not client-side) and asynchronously
                    var allDone = await db.TestResults
                        .Where(tr => tr.SubmissionId == submissionId)
                        .AllAsync(tr => tr.Status != "Pending", stoppingToken);

                    if (allDone)
                    {
                        // If you want: set Completed only if not already set
                        if (testResult.Submission != null)
                        {
                            testResult.Submission.Status = "Completed";
                        }
                        else
                        {
                            // Fallback: load submission and update
                            var sub = await db.Submissions.FindAsync(new object[] { submissionId }, stoppingToken);
                            if (sub != null) sub.Status = "Completed";
                        }
                    }
                }

            }
            // Wait before next poll
            await db.SaveChangesAsync(stoppingToken);
            await System.Threading.Tasks.Task.Delay(20000, stoppingToken); // 2 seconds
            }
        }
    }
}