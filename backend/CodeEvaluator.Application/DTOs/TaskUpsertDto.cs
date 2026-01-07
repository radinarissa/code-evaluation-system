namespace CodeEvaluator.Application.DTOs
{
    public class TaskUpsertDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = "";

        public int MaxExecutionTimeMs { get; set; }
        public int MemoryLimitKb { get; set; }
        public int MaxDiskUsageMb { get; set; }
        public decimal MaxPoints { get; set; }

        public List<TaskTestCaseUpsertDto> TestCases { get; set; } = new();
    }

    public class TaskTestCaseUpsertDto
    {
        public string Name { get; set; } = default!;
        public string Input { get; set; } = "";
        public string ExpectedOutput { get; set; } = "";
        public bool IsPublic { get; set; }
        public int ExecutionOrder { get; set; }
        public int Points { get; set; }
    }
}