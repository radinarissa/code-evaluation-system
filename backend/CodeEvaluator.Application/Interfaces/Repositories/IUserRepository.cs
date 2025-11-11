using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByMoodleIdAsync(int moodleId);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByCourseIdAsync(int courseId);
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
}