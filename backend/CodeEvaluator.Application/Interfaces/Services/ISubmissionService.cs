using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface ISubmissionService
    {

        Task<Submission> CreateSubmissionAndRunJudge0Async(SubmissionRequestDto dto);
        Task<Submission?> GetSubmissionByIdAsync(int id);
       SubmissionResponseDto ConvertSubmissiontoSubmissionResponseDto(Submission submission);
       List<Submission> GetAllSubmissions();
       Task<Status> DeleteSubmissionAsync(int id);

       public enum Status
       {
        Success,
        NotFound,
        Error
       }

    }
}
