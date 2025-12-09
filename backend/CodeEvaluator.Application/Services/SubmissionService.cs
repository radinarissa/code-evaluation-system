using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Infrastructure.Data;

namespace CodeEvaluator.Application.Services
{
    internal class SubmissionService : ISubmissionService
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
            var submission = new Submission
            {
                TaskId = dto.MoodleAssignmentId,
                Code = dto.SourceCode,
                UserId = dto.MoodleUserId,
                AttemptNumber = dto.MoodleAttemptId.GetValueOrDefault()
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            var judgeResult = await _judge0Service.ExecuteCodeAsync(submission);

            var result = new TestResult
            {
                SubmissionId = submission.Id,
                Output = judgeResult.Stdout,
                ErrorMessage = judgeResult.Stderr,
                Status = judgeResult.Status.Description,
                ExecutionTime = judgeResult.Time,
                MemoryUsage = judgeResult.Memory
            };

            _db.TestResults.Add(result);
            await _db.SaveChangesAsync();

            submission.TestResults.Add(result);
            return submission;
        }
    }
}
