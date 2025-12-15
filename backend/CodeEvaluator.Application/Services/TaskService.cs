using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Infrastructure.Data;

namespace CodeEvaluator.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Domain.Entities.Task> CreateAssignmentAsync(TaskRequestDto dto)
        {
            var assignment = new Domain.Entities.Task
            {
                MoodleAssignmentId = dto.MoodleAssignmentId,
                MoodleAssignmentName = dto.Name,
                Description = dto.Description,
                CreationDate = DateTime.UtcNow,
                MaxPoints = dto.MaxPoints,
                TimeLimitMs = dto.MaxExecutionTimeMs,
                DiskLimitMb = dto.MaxDiskUsageMb,
                CreatedBy = dto.CreatedByUserid,
                DueDate = dto.DueDate,
                IsActive = dto.IsActive,
                UpdatedAt = dto.UpdatedAt,
                Title = dto.Title,
                MemoryLimitMb = dto.MemoryLimitMb,
                CourseId = dto.CourseId
            };

            _db.Tasks.Add(assignment);
            await _db.SaveChangesAsync();
            return assignment;
        }
    }
}
