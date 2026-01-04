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
                TimeLimitS = dto.MaxExecutionTimeMs,
                DiskLimitKb = dto.MaxDiskUsageMb,
                CreatedBy = dto.CreatedByUserid,
                DueDate = dto.DueDate,
                IsActive = dto.IsActive,
                UpdatedAt = dto.UpdatedAt,
                Title = dto.Title,
                MemoryLimitKb = dto.MemoryLimitMb,
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
            // map Moodle teacher → backend user row
            //var teacher = await _db.Users.SingleOrDefaultAsync(u => u.MoodleId == dto.MoodleTeacherUserId);
            // var teacher = await _userService.UpsertFromMoodleAsync(dto.Teacher);
            // if (teacher == null)
            //     throw new InvalidOperationException($"Teacher with MoodleId={dto.MoodleTeacherUserId} not found in backend DB.");

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

            var task = await _db.Tasks
                .Include(t => t.TestCases)
                .SingleOrDefaultAsync(t => t.MoodleAssignmentId == dto.MoodleAssignmentId);

            if (task == null)
            {
                task = new Domain.Entities.Task
                {
                    MoodleCourseId = dto.MoodleCourseId,
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
                task.MoodleCourseId = dto.MoodleCourseId;
                task.MoodleAssignmentName = dto.MoodleAssignmentName;
                task.Title = dto.Title;
                task.Description = dto.Description;
                task.MaxPoints = dto.MaxPoints;
                task.TimeLimitS = dto.TimeLimitS;
                task.MemoryLimitKb = dto.MemoryLimitKb;
                task.DiskLimitKb = dto.DiskLimitKb;
                task.StackLimitKb = dto.StackLimitKb;
                task.UpdatedAt = DateTime.UtcNow;

                _db.TestCases.RemoveRange(task.TestCases);
                task.TestCases.Clear();
            }

            foreach (var tc in dto.TestCases.OrderBy(x => x.ExecutionOrder))
            {
                task.TestCases.Add(new TestCase
                {
                    TaskId = task.Id,
                    Name = tc.Name,
                    Input = tc.Input ?? "",
                    ExpectedOutput = tc.ExpectedOutput ?? "",
                    IsPublic = tc.IsPublic,
                    Points = tc.Points,
                    ExecutionOrder = tc.ExecutionOrder
                });
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
                MaxDiskUsageMb = task.DiskLimitKb,
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
                MaxDiskUsageMb = t.DiskLimitKb,
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
    }
}
