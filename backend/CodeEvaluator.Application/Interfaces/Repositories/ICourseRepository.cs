using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetByMoodleCourseIdAsync(int moodleCourseId);
    Task<Course?> GetCourseWithTasksAsync(int courseId);
    Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int userId);
}