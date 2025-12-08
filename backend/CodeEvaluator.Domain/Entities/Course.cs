namespace CodeEvaluator.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public int MoodleCourseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<CourseEnrollment> Enrollments { get; set; } = new List<CourseEnrollment>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}