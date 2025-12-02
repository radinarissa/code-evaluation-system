using CodeEvaluator.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionController : ControllerBase
    {
        /// <summary>
        /// Returns all submissions.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllSubmissions()
        {
            // TODO: Return a list of SubmissionResponseDto
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Returns a single submission by its id.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetSubmissionById(int id)
        {
            // TODO: Return SubmissionResponseDto for the given id
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Creates a new submission for a task (with Moodle integration fields).
        /// </summary>
        [HttpPost]
        public IActionResult CreateSubmission([FromBody] SubmissionRequestDto request)
        {
            // TODO:
            //  - validate request
            //  - run tests in sandbox
            //  - calculate score and feedback
            //  - optionally sync result back to Moodle
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Updates an existing submission (if business rules allow it).
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateSubmission(int id, [FromBody] SubmissionRequestDto request)
        {
            // TODO: Update submission data if needed
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Deletes a submission by its id.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteSubmission(int id)
        {
            // TODO: Delete submission with the given id
            return StatusCode(501, "Not implemented");
        }
    }
}
