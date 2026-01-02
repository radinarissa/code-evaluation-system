namespace CodeEvaluator.Application.DTOs
{
    public class MoodleTaskUpsertDto
    {
        public int MoodleCourseId { get; set; }
        public int MoodleAssignmentId { get; set; }
        public string MoodleAssignmentName { get; set; } = default!;

        public MoodleUserDto? Teacher { get; set; } = new();
        public int? MoodleTeacherUserId { get; set; } // Moodle user id
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        public decimal MaxPoints { get; set; } = 10;

        public int TimeLimitS { get; set; } = 3;
        public int MemoryLimitKb { get; set; } = 262144;
        public int DiskLimitKb { get; set; } = 256;
        public int? StackLimitKb { get; set; }

        public List<MoodleTestCaseDto> TestCases { get; set; } = new();
    }

    public class MoodleTestCaseDto
    {
        public string Name { get; set; } = default!;
        public string Input { get; set; } = "";
        public string ExpectedOutput { get; set; } = "";
        public bool IsPublic { get; set; } = false;
        public decimal Points { get; set; } = 1;
        public int ExecutionOrder { get; set; } = 1;
    }
}