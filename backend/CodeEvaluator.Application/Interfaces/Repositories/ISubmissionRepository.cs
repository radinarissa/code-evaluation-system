using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<IEnumerable<Submission>> GetSubmissionsByTaskIdAsync(int taskId);
    Task<IEnumerable<Submission>> GetSubmissionsByUserIdAsync(int userId);
    Task<IEnumerable<Submission>> GetSubmissionsByTaskAndUserAsync(int taskId, int userId);
    Task<Submission?> GetSubmissionWithTestResultsAsync(int submissionId);
    Task<IEnumerable<Submission>> GetPendingSubmissionsAsync();
}