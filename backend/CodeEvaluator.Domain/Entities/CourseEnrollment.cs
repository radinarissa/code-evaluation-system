namespace CodeEvaluator.Domain.Entities;

public class CourseEnrollment
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty; 
    public DateTime EnrolledAt { get; set; }

    public Course Course { get; set; } = null!;
    public User User { get; set; } = null!;
}