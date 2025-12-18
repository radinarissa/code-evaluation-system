using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface IJudge0Service
    {
        Task<Judge0ResultDto> ExecuteCodeAsync(Judge0SubmissionDTO submission);
        Task<List<String>> ExecuteBatchCodeAsync(List<Judge0SubmissionDTO> submissions);
        Task<List<Judge0ResultDto>> GetSubbmisionBatchAsync(List<string> tokens);
        Task<Judge0ResultDto> GetSubmissionResponseDtoFromJudge0(string token);
        
    }
}
