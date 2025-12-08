using CodeEvaluator.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        /// <summary>
        /// Returns all tasks.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllTasks()
        {
            // TODO: Return a list of TaskResponseDto
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Returns a single task by its id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTaskById(int id)
        {
            // TODO: Return TaskResponseDto for the given id
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Creates a new task using the provided data.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateTask([FromBody] TaskRequestDto request)
        {
            // TODO: Map TaskRequestDto to domain model and create a new task
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateTask(int id, [FromBody] TaskRequestDto request)
        {
            // TODO: Map TaskRequestDto to domain model and update the task with the given id
            return StatusCode(501, "Not implemented");
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
    }
}
