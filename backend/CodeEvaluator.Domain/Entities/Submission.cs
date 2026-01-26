namespace CodeEvaluator.Domain.Entities;

public class Submission
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public int AttemptNumber { get; set; } = 1;
    public DateTime SubmissionTime { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public decimal? FinalGrade { get; set; }
    public string? Feedback { get; set; }
    public string? CompilationOutput { get; set; }
    public DateTime? EvaluationStartedAt { get; set; }
    public DateTime? EvaluationCompletedAt { get; set; }
    public int? MoodleSubmissionId { get; set; }
    public int? MoodleAttemptNumber { get; set; }
    public string? MoodleSyncStatus { get; set; }
    public string? MoodleSyncOutput { get; set; }
    public DateTime? MoodleSyncCreatedAt { get; set; }

    public Task Task { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}