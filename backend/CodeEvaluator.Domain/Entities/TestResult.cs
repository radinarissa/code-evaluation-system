namespace CodeEvaluator.Domain.Entities;

public class TestResult
{
    public int Id { get; set; }
    public int TestCaseId { get; set; }
    public int SubmissionId { get; set; }
    public string Status { get; set; } = string.Empty; 
    public float ExecutionTime { get; set; }
    public float? MemoryUsage { get; set; }
    public decimal? DiskUsedMb { get; set; }
    public string? Output { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Judge0Token { get; set; }
    public DateTime CreatedAt { get; set; }

    public TestCase TestCase { get; set; } = null!;
    public Submission Submission { get; set; } = null!;
}