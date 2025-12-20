using System.Threading.Tasks;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeEvaluator.Application.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ApplicationDbContext _db;
        private readonly IJudge0Service _judge0Service;

        public SubmissionService(ApplicationDbContext db, IJudge0Service judge0Service)
        {
            _db = db;
            _judge0Service = judge0Service;
        }

        public async Task<Submission> CreateSubmissionAndRunJudge0Async(SubmissionRequestDto dto)
        {
            var task = await _db.Tasks.FindAsync(dto.TaskId);

            if (task == null) throw new Exception("Task not found");

            var user = await _db.Users.SingleOrDefaultAsync(u => u.MoodleId == dto.MoodleUserId);

            if (user == null) throw new InvalidOperationException($"User with MoodleId={dto.MoodleUserId} not found in backend DB.");

            var highestAttempt = await _db.Submissions
                .Where(a => a.UserId == user.Id && a.TaskId == task.Id)
                .MaxAsync(a => (int?)a.AttemptNumber) ?? 0;

            highestAttempt++;

           var submission = new Submission
            {   
             Task = task,                     
             Code = dto.SourceCode,
             //UserId = dto.MoodleUserId,
             UserId = user.Id,
             MoodleSubmissionId = dto.MoodleSubmissionId,
             AttemptNumber = highestAttempt,
             SubmissionTime = DateTime.UtcNow,
             Status = "Pending",
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            List<TestCase> testCases = await _db.TestCases
                .Where(tc => tc.TaskId == task.Id)
                .ToListAsync();

            //wtf is moodleassignmentid meant to be?

            // Domain.Entities.Task task = await _db.Tasks
            //     .FirstOrDefaultAsync(t => t.Id == dto.MoodleAssignmentId);

            // if (task == null)
            // {
            //   throw new InvalidOperationException($"Task with ID {dto.MoodleAssignmentId} not found.");
            // }
            List<Judge0SubmissionDTO> judge0Submissions = new List<Judge0SubmissionDTO>();
            List<TestResult> testResults = new List<TestResult>();

            foreach (var testCase in testCases)
            {
            Judge0SubmissionDTO judge0sub = new Judge0SubmissionDTO
            {
                SourceCode = dto.SourceCode,
                LanguageId = dto.Language,
                CpuTimeLimit = task.TimeLimitS,
                MemoryLimit = task.MemoryLimitKb,
                StackLimit = task.StackLimitKb,
                MaxFileSize = task.DiskLimitKb,
                StdIn = testCase.Input,
                ExpectedOutput = testCase.ExpectedOutput
            };
            judge0Submissions.Add(judge0sub);

            TestResult testResult = new TestResult
            {
                Submission = submission,
                TestCase = testCase,
                Status = "Pending"
            };
            testResults.Add(testResult);
            }

            _db.TestResults.AddRange(testResults);

          
            List<string> judgeResult = await _judge0Service.ExecuteBatchCodeAsync(judge0Submissions);
    //          var submissionsinfo = await _judge0Service.GetSubbmisionBatchAsync(judgeResult); //For some reason returns 0?
           
           //should work if order is preserved
           if (judgeResult.Count != testResults.Count)
            {
              throw new InvalidOperationException("Judge0 token count does not match test result count");
            }
            for (int i = 0; i < testResults.Count;i++)
            {
                 testResults[i].Judge0Token = judgeResult[i];
            }

           
           
            await _db.SaveChangesAsync();
            return submission;
        }

        public async Task<Submission?> GetSubmissionByIdAsync(int id)
        {
            var submission = await _db.Submissions
        .FirstOrDefaultAsync(s => s.Id == id);

            return submission;
   
        }
        public SubmissionResponseDto ConvertSubmissiontoSubmissionResponseDto(Submission submission)
        {
            // Map Submission entity to SubmissionResponseDto
            var responseDto = new SubmissionResponseDto
            {
                Id = submission.Id,
                TaskId = submission.TaskId,
                StudentId = submission.UserId,
                Language = "C#",
                SourceCode = submission.Code,
                Status = submission.Status,
                Score = submission.FinalGrade,
                Feedback = submission.Feedback,
                SubmittedAt = submission.SubmissionTime
            };

            return responseDto;
        }
        public List<Submission> GetAllSubmissions()
        {
            return _db.Submissions.ToList();
        }
        public async Task<ISubmissionService.Status> DeleteSubmissionAsync(int id)
        {
            Console.WriteLine("looking for id: "+id);
            var submission = await _db.Submissions.FindAsync(id);
            if (submission == null)
            {
                return ISubmissionService.Status.NotFound;
            }
            Console.WriteLine("submission found");
            _db.Submissions.Remove(submission);
            await _db.SaveChangesAsync();
            return ISubmissionService.Status.Success;
        }

    
    }
}
