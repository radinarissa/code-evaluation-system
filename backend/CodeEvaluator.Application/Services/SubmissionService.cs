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
            var submission = new Submission
            {
                TaskId = dto.MoodleAssignmentId,
                Code = dto.SourceCode,
                UserId = dto.MoodleUserId,
                AttemptNumber = dto.MoodleAttemptId.GetValueOrDefault()
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            Judge0SubmissionDTO judge0sub = new Judge0SubmissionDTO
            {

                SourceCode = dto.SourceCode,
                LanguageId = dto.Language,
                CpuTimeLimit = dto.CpuTimeLimit,
                MemoryLimit = dto.MemoryLimit,
                StackLimit = dto.StackLimit,
                StdIn = dto.StdIn,
                ExpectedOutput = dto.ExpectedOutput
            };

            var judgeResult = await _judge0Service.ExecuteCodeAsync(judge0sub);

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
            var submission = await _db.Submissions.FindAsync(id);
            if (submission == null)
            {
                return ISubmissionService.Status.NotFound;
            }

            _db.Submissions.Remove(submission);
            await _db.SaveChangesAsync();
            return ISubmissionService.Status.Success;
        }

    
    }
}
