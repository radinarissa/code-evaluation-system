using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    internal interface ITaskService
    {
        Task<Domain.Entities.Task> CreateAssignmentAsync(TaskRequestDto dto);
    }
}
