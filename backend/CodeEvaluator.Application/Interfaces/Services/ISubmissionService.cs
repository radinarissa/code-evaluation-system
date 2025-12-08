using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    internal interface ISubmissionService
    {
        Task<Domain.Entities.Submission> CreateAssignmentAsync(SubmissionRequestDto dto);
    }
}
