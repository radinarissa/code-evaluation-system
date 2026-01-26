using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Returns all tasks.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Returns a single task by its id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        /// <summary>
        /// Creates a new task using the provided data.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> CreateTask([FromBody] TaskRequestDto dto)
        // {
        //     var assignment = await _taskService.CreateAssignmentAsync(dto);
        //     return Ok(assignment);
        // }
        public async Task<IActionResult> CreateTask([FromBody] TaskUpsertDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized("Missing/invalid user id claim.");

            var created = await _taskService.CreateTaskAsync(dto, userId);
            return Ok(created);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpsertDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized("Missing/invalid user id claim.");

            var updated = await _taskService.UpdateTaskAsync(id, dto, userId);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a task by its id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTask(int id)
        {
            // TODO: Delete task with the given id
            return StatusCode(501, "Not implemented");
        }
        
        public record MoodleUpsertResponseDto(int TaskId, int? MoodleAssignmentId);

        [AllowAnonymous]
        [HttpPost("moodle/upsert")] 
        public async Task<IActionResult> UpsertFromMoodle([FromBody] MoodleTaskUpsertDto dto)
        {
            var task = await _taskService.UpsertFromMoodleAsync(dto);
            return Ok(new MoodleUpsertResponseDto(task.Id, task.MoodleAssignmentId));
        }
    }
}
