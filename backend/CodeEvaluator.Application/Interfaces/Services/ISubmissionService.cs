using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface ISubmissionService
    {
        Task<Submission> CreateSubmissionAndRunJudge0Async(SubmissionRequestDto dto);
    }
}
