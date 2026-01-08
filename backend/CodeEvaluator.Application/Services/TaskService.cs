using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CodeEvaluator.Domain.Entities;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Net.Sockets;

namespace CodeEvaluator.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        public TaskService(ApplicationDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
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
                TimeLimitS = dto.MaxExecutionTimeMs / 1000,
                DiskLimitKb = dto.MaxDiskUsageMb * 1024,
                CreatedBy = dto.CreatedByUserid,
                DueDate = dto.DueDate,
                IsActive = dto.IsActive,
                UpdatedAt = dto.UpdatedAt,
                Title = dto.Title,
                MemoryLimitKb = dto.MemoryLimitMb * 1024,
                MoodleCourseId = dto.CourseId,
                TestCases = dto.TestCases.Select(x => new Domain.Entities.TestCase
                {
                    Name = x.Name,
                    Input = x.InputFilePath,
                    ExpectedOutput = x.OutputFilePath,
                }).ToList()
            };

            _db.Tasks.Add(assignment);
            await _db.SaveChangesAsync();
            return assignment;
        }

        public async Task<Domain.Entities.Task> UpsertFromMoodleAsync(MoodleTaskUpsertDto dto)
        {
            var hasTaskId = dto.TaskId.HasValue && dto.TaskId.Value > 0;
            if (!hasTaskId)
            {
                if (!dto.MoodleAssignmentId.HasValue || dto.MoodleAssignmentId.Value <= 0)
                    throw new InvalidOperationException("MoodleAssignmentId is required when TaskId is not provided.");
            }

            MoodleUserDto teacherDto;

            if (dto.Teacher != null && dto.Teacher.MoodleId > 0)
            {
                teacherDto = dto.Teacher;
            }
            else if (dto.MoodleTeacherUserId.HasValue && dto.MoodleTeacherUserId.Value > 0)
            {
                // Minimal teacher info
                teacherDto = new MoodleUserDto
                {
                    MoodleId = dto.MoodleTeacherUserId.Value,
                    Role = "Teacher",
                    Username = "",
                    Email = "",
                    FirstName = "",
                    LastName = ""
                };
            }
            else
            {
                throw new InvalidOperationException("Teacher MoodleId is required (teacher.moodleId or moodleTeacherUserId).");
            }

            teacherDto.Role = string.IsNullOrWhiteSpace(teacherDto.Role) ? "Teacher" : teacherDto.Role;
            var teacher = await _userService.UpsertFromMoodleAsync(teacherDto);

            Domain.Entities.Task? task = null;

            // Prefer TaskId if provided (binding existing backend task)
            if (hasTaskId)
            {
                task = await _db.Tasks.Include(t => t.TestCases)
                    .SingleOrDefaultAsync(t => t.Id == dto.TaskId!.Value);
            }
            else
            {
                task = await _db.Tasks.Include(t => t.TestCases)
                    .SingleOrDefaultAsync(t => t.MoodleAssignmentId == dto.MoodleAssignmentId!.Value);
            }

            if (task == null)
            {
                // If creating from Moodle, require mapping fields:
                if (!dto.MoodleCourseId.HasValue || dto.MoodleCourseId.Value <= 0) throw new InvalidOperationException("MoodleCourseId is required when creating a task from Moodle.");
                if (string.IsNullOrWhiteSpace(dto.MoodleAssignmentName)) throw new InvalidOperationException("MoodleAssignmentName is required when creating a task from Moodle.");

                task = new Domain.Entities.Task
                {
                    MoodleCourseId = dto.MoodleCourseId.Value,
                    MoodleAssignmentId = dto.MoodleAssignmentId,
                    MoodleAssignmentName = dto.MoodleAssignmentName,
                    Title = dto.Title,
                    Description = dto.Description,
                    MaxPoints = dto.MaxPoints,
                    TimeLimitS = dto.TimeLimitS,
                    MemoryLimitKb = dto.MemoryLimitKb,
                    DiskLimitKb = dto.DiskLimitKb,
                    StackLimitKb = dto.StackLimitKb,
                    CreatedBy = teacher.Id,
                    IsActive = true,
                    CreationDate = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _db.Tasks.Add(task);
                await _db.SaveChangesAsync();
            }
            else
            {
                //Only update mapping if present
                if (dto.MoodleCourseId.HasValue && dto.MoodleCourseId.Value > 0) task.MoodleCourseId = dto.MoodleCourseId.Value;

                if (dto.MoodleAssignmentId.HasValue && dto.MoodleAssignmentId.Value > 0) task.MoodleAssignmentId = dto.MoodleAssignmentId.Value;

                if (!string.IsNullOrWhiteSpace(dto.MoodleAssignmentName)) task.MoodleAssignmentName = dto.MoodleAssignmentName;

                // task.MoodleCourseId = dto.MoodleCourseId;
                // task.MoodleAssignmentId = dto.MoodleAssignmentId;
                // task.MoodleAssignmentName = dto.MoodleAssignmentName;
                task.Title = dto.Title;
                task.Description = dto.Description;
                task.MaxPoints = dto.MaxPoints;
                task.TimeLimitS = dto.TimeLimitS;
                task.MemoryLimitKb = dto.MemoryLimitKb;
                task.DiskLimitKb = dto.DiskLimitKb;
                task.StackLimitKb = dto.StackLimitKb;
                task.UpdatedAt = DateTime.UtcNow;

                // _db.TestCases.RemoveRange(task.TestCases);
                // task.TestCases.Clear();
            }
            if (dto.TestCases != null)
            {
                if (dto.TestCases.Count == 0)
                    throw new InvalidOperationException("Refusing to replace testcases with empty list.");

                _db.TestCases.RemoveRange(task.TestCases);
                task.TestCases.Clear();

                foreach (var tc in dto.TestCases.OrderBy(x => x.ExecutionOrder))
                {
                    task.TestCases.Add(new TestCase
                    {
                        Name = tc.Name,
                        Input = tc.Input ?? "",
                        ExpectedOutput = tc.ExpectedOutput ?? "",
                        IsPublic = tc.IsPublic,
                        Points = tc.Points,
                        ExecutionOrder = tc.ExecutionOrder
                    });
                }
            }

            await _db.SaveChangesAsync();
            return task;
        }


        public async Task<TaskResponseDto?> GetTaskByIdAsync(int id)
        {
            var task = await _db.Tasks
                .Include(t => t.TestCases)
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == id);

            if (task == null) return null;

            return new TaskResponseDto
            {
                Id = task.Id,
                Name = task.Title ?? task.MoodleAssignmentName ?? "",
                Description = task.Description ?? "",
                MaxPoints = task.MaxPoints,
                MaxExecutionTimeMs = task.TimeLimitS * 1000, //convert to ms
                MemoryLimitKb = task.MemoryLimitKb,
                MaxDiskUsageMb = task.DiskLimitKb / 1024,
                CreatedAt = task.CreationDate,

                TestCases = task.TestCases
                    .OrderBy(tc=> tc.ExecutionOrder)
                    .Select(tc => new TaskTestCaseDto
                    {
                        Name = tc.Name,
                        Input = tc.Input ?? "",
                        ExpectedOutput = tc.ExpectedOutput ?? "",
                        IsPublic = tc.IsPublic,
                        ExecutionOrder = tc.ExecutionOrder,
                        Points = tc.Points
                    }).ToList()
            };
        }

        public async Task<List<TaskResponseDto>> GetAllTasksAsync()
        {
            var tasks = await _db.Tasks
                .Include(t => t.TestCases)
                .AsNoTracking()
                .OrderByDescending(t => t.UpdatedAt)
                .ToListAsync();

            return tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Name = t.Title ?? t.MoodleAssignmentName ?? "",
                Description = t.Description ?? "",
                MaxPoints = t.MaxPoints,
                MaxExecutionTimeMs = t.TimeLimitS * 1000,
                MemoryLimitKb = t.MemoryLimitKb,
                MaxDiskUsageMb = t.DiskLimitKb / 1024,
                CreatedAt = t.CreationDate,
                TestCases = t.TestCases
                    .OrderBy(tc=> tc.ExecutionOrder)
                    .Select(tc => new TaskTestCaseDto
                    {
                        Name = tc.Name,
                        Input = tc.Input ?? "",
                        ExpectedOutput = tc.ExpectedOutput ?? "",
                        IsPublic = tc.IsPublic,
                        ExecutionOrder = tc.ExecutionOrder,
                        Points = tc.Points
                    }).ToList()
            }).ToList();
        }


        public async Task<TaskResponseDto> CreateTaskAsync(TaskUpsertDto dto, int createdByUserId)
        {
            var task = new Domain.Entities.Task
            {
                Title = dto.Title,
                Description = dto.Description ?? "",
                MaxPoints = dto.MaxPoints,
                TimeLimitS = dto.MaxExecutionTimeMs / 1000,
                MemoryLimitKb = dto.MemoryLimitKb,
                DiskLimitKb = dto.MaxDiskUsageMb * 1024,
                CreationDate = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = createdByUserId,
                MoodleCourseId = 0 // TODO later
            };

            foreach (var tc in dto.TestCases.OrderBy(x => x.ExecutionOrder))
            {
                task.TestCases.Add(new TestCase
                {
                    Name = tc.Name,
                    Input = tc.Input,
                    ExpectedOutput = tc.ExpectedOutput,
                    IsPublic = tc.IsPublic,
                    ExecutionOrder = tc.ExecutionOrder,
                    Points = tc.Points
                });
            }

            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id) ?? throw new Exception("Created task not found");
        }

        public async Task<TaskResponseDto?> UpdateTaskAsync(int id, TaskUpsertDto dto, int updatedByUserId)
        {
            var task = await _db.Tasks.Include(t => t.TestCases).SingleOrDefaultAsync(t => t.Id == id);
            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description ?? "";
            task.MaxPoints = dto.MaxPoints;
            task.TimeLimitS = dto.MaxExecutionTimeMs / 1000;
            task.MemoryLimitKb = dto.MemoryLimitKb;
            task.DiskLimitKb = dto.MaxDiskUsageMb * 1024;
            task.UpdatedAt = DateTime.UtcNow;

            _db.TestCases.RemoveRange(task.TestCases);
            task.TestCases.Clear();

            foreach (var tc in dto.TestCases.OrderBy(x => x.ExecutionOrder))
            {
                task.TestCases.Add(new TestCase
                {
                    Name = tc.Name,
                    Input = tc.Input,
                    ExpectedOutput = tc.ExpectedOutput,
                    IsPublic = tc.IsPublic,
                    ExecutionOrder = tc.ExecutionOrder,
                    Points = tc.Points
                });
            }

            await _db.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id);
        }
    }
}
