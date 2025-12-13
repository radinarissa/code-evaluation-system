using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface IJudge0Service
    {
        Task<Judge0ResultDto> ExecuteCodeAsync(Judge0SubmissionDTO submission);
     
        
    }
}
