namespace CodeEvaluator.Domain.Entities;

public class TestCase
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public decimal Points { get; set; }
    public int ExecutionOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Task Task { get; set; } = null!;
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}