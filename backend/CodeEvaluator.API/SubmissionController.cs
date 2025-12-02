using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSubmissions()
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpGet("{id}")]
        public IActionResult GetSubmissionById(int id)
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpPost]
        public IActionResult CreateSubmission()
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubmission(int id)
        {
            return StatusCode(501, "Not implemented");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubmission(int id)
        {
            return StatusCode(501, "Not implemented");
        }
    }
}
