namespace CodeEvaluator.API.DTOs
{
    /// <summary>
    /// Output model returned by the API when reading task data.
    /// </summary>
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public List<TaskTestCaseDto> TestCases { get; set; } = new();

        public int MaxExecutionTimeMs { get; set; }

        public int MaxDiskUsageMb { get; set; }

        public int MaxPoints { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
