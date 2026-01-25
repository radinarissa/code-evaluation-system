using DomainTask = CodeEvaluator.Domain.Entities.Task;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface ITaskRepository : IRepository<DomainTask>
{
    Task<IEnumerable<DomainTask>> GetTasksByCourseIdAsync(int courseId);
    Task<IEnumerable<DomainTask>> GetActiveTasksAsync();
    Task<DomainTask?> GetTaskWithTestCasesAsync(int taskId);
    Task<DomainTask?> GetTaskByMoodleAssignmentIdAsync(int moodleAssignmentId);
}