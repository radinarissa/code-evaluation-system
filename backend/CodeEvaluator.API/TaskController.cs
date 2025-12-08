using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpPost]
        public IActionResult CreateTask()
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id)
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            return StatusCode(501, "Not implemented");
        }
    }
}
