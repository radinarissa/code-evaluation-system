using CodeEvaluator.Application.DTOs;
using System.Threading.Tasks;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface ITaskService
    {
        public Task<Domain.Entities.Task> CreateAssignmentAsync(TaskRequestDto dto);

        Task<Domain.Entities.Task> UpsertFromMoodleAsync(Application.DTOs.MoodleTaskUpsertDto dto);

        Task<TaskResponseDto?> GetTaskByIdAsync(int id);

        Task<List<TaskResponseDto>> GetAllTasksAsync();
    }
}
