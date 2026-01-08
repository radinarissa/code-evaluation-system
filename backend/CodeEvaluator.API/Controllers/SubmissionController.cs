using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly IUserService _userService;
        
        public SubmissionController(ISubmissionService submissionService, IUserService userService)
        {
            _submissionService = submissionService;
            _userService = userService;
        }

        /// <summary>
        /// Returns all submissions.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubmissionResponseDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllSubmissions()
        {
            List<Submission> submissions = _submissionService.GetAllSubmissions();
            var responseDtos = new List<SubmissionResponseDto>();
            foreach (var i in submissions)
            {
                SubmissionResponseDto responseDto = _submissionService.ConvertSubmissiontoSubmissionResponseDto(i);
                responseDtos.Add(responseDto);
            }

            return Ok(responseDtos);

        }

        [Route("GetSubmissionsByTaskId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubmissionResponseDto>), StatusCodes.Status200OK)]
        public IActionResult GetSubmissionsByTaskId(int taskId)
        {
            List<Submission> submissions = _submissionService.GetSubmissionsByTaskId(taskId);
            var responseDtos = new List<SubmissionResponseDto>();
            foreach (var i in submissions)
            {
                SubmissionResponseDto responseDto = _submissionService.ConvertSubmissiontoSubmissionResponseDto(i);
                responseDtos.Add(responseDto);
            }

            return Ok(responseDtos);

        }

        /// <summary>
        /// Returns a single submission by its id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubmissionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
           var submission = await _submissionService.GetSubmissionByIdAsync(id);
           if (submission == null)
                return NotFound();
            SubmissionResponseDto responseDto = _submissionService.ConvertSubmissiontoSubmissionResponseDto(submission);
              return Ok(responseDto);
          
        }

        /// <summary>
        /// Creates a new submission for a task (with Moodle integration fields).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSubmission([FromBody] SubmissionRequestDto dto)
        {
            // var submission = await _submissionService.CreateSubmissionAndRunJudge0Async(dto);
            // return Ok(submission);
            // If Moodle didn't send user info, fail clearly
            if (dto.User == null || dto.User.MoodleId <= 0)
                return BadRequest("Missing dto.user.moodleId (Moodle user info is required).");

            var dbUser = await _userService.UpsertFromMoodleAsync(dto.User);
            
            dto.StudentId = dbUser.Id;

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
        public async Task<IActionResult> UpdateSubmission(int id, [FromBody] SubmissionRequestDto request)
        {
        

            return StatusCode(501, "Not implemented");
        }

        /// <summary>
        /// Deletes a submission by its id.
        /// </summary> 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            // TODO: Delete submission with the given id
            var a = await _submissionService.DeleteSubmissionAsync(id);
            return a switch
            {
                ISubmissionService.Status.Success => NoContent(),
                ISubmissionService.Status.NotFound => NotFound(),
                _ => StatusCode(500, "An error occurred while deleting the submission.")
            };
        }
    }
}
