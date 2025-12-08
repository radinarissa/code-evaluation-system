namespace CodeEvaluator.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public int MoodleId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<CourseEnrollment> Enrollments { get; set; } = new List<CourseEnrollment>();
    public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<ReferenceSolution> ReferenceSolutions { get; set; } = new List<ReferenceSolution>();
}