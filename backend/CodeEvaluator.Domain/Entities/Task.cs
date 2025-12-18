namespace CodeEvaluator.Domain.Entities;

public class Task
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal MaxPoints { get; set; }
    public decimal? TimeLimitS { get; set; }
    public int? MemoryLimitKb { get; set; }
    public int? DiskLimitKb { get; set; }
    public int? StackLimitKb { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? MoodleAssignmentId { get; set; }
    public string? MoodleAssignmentName { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Course Course { get; set; } = null!;
    public User Creator { get; set; } = null!;
    public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
    public ICollection<ReferenceSolution> ReferenceSolutions { get; set; } = new List<ReferenceSolution>();
    public ICollection<AdditionalFile> AdditionalFiles { get; set; } = new List<AdditionalFile>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}