namespace CodeEvaluator.Application.DTOs
{
    /// <summary>
    /// Input model used when a student submits a solution for a task.
    /// </summary>
    public class SubmissionRequestDto
    {
        /// <summary>
        /// Internal identifier of the task in the evaluation system.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Internal or external student identifier in the evaluation system.
        /// </summary>
        public int StudentId { get; set; } = default!;

        /// <summary>
        /// Programming language of the submitted solution (e.g. C#).
        /// </summary>
        public int Language { get; set; } = 51;

        /// <summary>
        /// Source code submitted by the student.
        /// </summary>
        public string SourceCode { get; set; } = default!;

        // === Moodle integration fields ===

        /// <summary>
        /// User identifier in Moodle.
        /// </summary>
        public int MoodleUserId { get; set; } = default!;

        /// <summary>
        /// Course identifier in Moodle.
        /// </summary>
        public string MoodleCourseId { get; set; } = default!;

        /// <summary>
        /// Submission identifier in Moodle.
        /// </summary>
        public int MoodleSubmissionId { get; set; } = default!;

        /// <summary>
        /// Optional attempt number in Moodle.
        /// </summary>
        public int? MoodleAttemptId { get; set; }

        /// <summary>
        /// Field to get the correct attempt number from Moodle.
        /// </summary>
        public int? MoodleAttemptNumber { get; set; }

        /// <summary>
        /// Send a full user object so the backend can upsert the student
        /// </summary>
        public MoodleUserDto User { get; set; } = new();
    }
}
