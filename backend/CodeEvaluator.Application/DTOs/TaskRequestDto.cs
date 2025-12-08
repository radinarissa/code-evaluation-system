namespace CodeEvaluator.Application.DTOs
{
    /// <summary>
    /// Input model used when creating or updating a programming task.
    /// </summary>
    public class TaskRequestDto
    {
        /// <summary>
        /// Name/title of the task.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Textual description of the task shown to students.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Collection of test cases used to evaluate submissions.
        /// </summary>
        public List<TaskTestCaseDto> TestCases { get; set; } = new();

        /// <summary>
        /// Reference C# solution code, if provided by the teacher.
        /// </summary>
        public string? ReferenceSolutionCode { get; set; }

        /// <summary>
        /// Maximum execution time per test case in milliseconds.
        /// </summary>
        public int MaxExecutionTimeMs { get; set; }

        /// <summary>
        /// Maximum disk usage per test case in megabytes.
        /// </summary>
        public int MaxDiskUsageMb { get; set; }

        /// <summary>
        /// Maximum score a student can get for this task.
        /// </summary>
        public int MaxPoints { get; set; }

        /// <summary>
        /// Optional Moodle course identifier for integration.
        /// </summary>
        public string? MoodleCourseId { get; set; }

        /// <summary>
        /// Optional Moodle assignment identifier for integration.
        /// </summary>
        public string? MoodleAssignmentId { get; set; }
    }
}
