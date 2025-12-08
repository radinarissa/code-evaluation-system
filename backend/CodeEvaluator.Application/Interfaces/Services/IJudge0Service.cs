using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    internal interface IJudge0Service
    {
        Task<SubmissionRequestDto> ExecuteCodeAsync(Submission submission);
    }
}
