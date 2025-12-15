using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface ITaskService
    {
        public Task<Domain.Entities.Task> CreateAssignmentAsync(TaskRequestDto dto);
    }
}
