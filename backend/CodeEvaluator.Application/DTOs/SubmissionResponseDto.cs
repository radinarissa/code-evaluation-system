namespace CodeEvaluator.Application.DTOs
{
    /// <summary>
    /// Output model returned by the API when reading submission data.
    /// </summary>
    public class SubmissionResponseDto
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int StudentId { get; set; } = default!;

        public string Language { get; set; } = default!;

        public string SourceCode { get; set; } = default!;

        /// <summary>
        /// Status of the submission, e.g. Pending, Compiled, FailedTests, Passed.
        /// </summary>
        public string Status { get; set; } = default!;

        /// <summary>
        /// Score awarded for this submission (0..MaxPoints).
        /// </summary>
        public decimal? Score { get; set; }

        /// <summary>
        /// Textual feedback that can be sent back to Moodle.
        /// </summary>
        public string Feedback { get; set; } = default!;

        public DateTime SubmittedAt { get; set; }
    }
}
