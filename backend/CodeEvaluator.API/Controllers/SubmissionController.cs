using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        /// <summary>
        /// Returns all submissions.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubmissionResponseDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllSubmissions()
        {
            // TODO: Return a list of SubmissionResponseDto
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Returns a single submission by its id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubmissionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSubmissionById(int id)
        {
            // TODO: Return SubmissionResponseDto for the given id
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Creates a new submission for a task (with Moodle integration fields).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSubmission([FromBody] SubmissionRequestDto dto)
        {
            var submission = await _submissionService.CreateSubmissionAndRunJudge0Async(dto);
            return Ok(submission);
        }

        /// <summary>
        /// Updates an existing submission (if business rules allow it).
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSubmission(int id, [FromBody] SubmissionRequestDto request)
        {
            // TODO: Update submission data if needed
            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Deletes a submission by its id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSubmission(int id)
        {
            // TODO: Delete submission with the given id
            return StatusCode(501, "Not implemented");
        }
    }
}
