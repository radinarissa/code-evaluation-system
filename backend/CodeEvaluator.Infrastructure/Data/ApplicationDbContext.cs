using Microsoft.EntityFrameworkCore;
using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
    public DbSet<Domain.Entities.Task> Tasks { get; set; }
    public DbSet<TestCase> TestCases { get; set; }
    public DbSet<ReferenceSolution> ReferenceSolutions { get; set; }
    public DbSet<AdditionalFile> AdditionalFiles { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<TestResult> TestResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.MoodleCourseId).IsRequired();
            entity.HasIndex(e => e.MoodleCourseId).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.AcademicYear).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Semester).HasMaxLength(255).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MoodleId).IsRequired();
            entity.HasIndex(e => e.MoodleId).IsUnique();
            entity.Property(e => e.Username).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FirstName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.ToTable("CourseEnrollments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Role).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EnrolledAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.CourseId, e.UserId }).IsUnique();
        });

        modelBuilder.Entity<Domain.Entities.Task>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.MaxPoints).HasPrecision(5, 2).HasDefaultValue(10.00m);
            entity.Property(e => e.TimeLimitMs).HasDefaultValue(5000);
            entity.Property(e => e.MemoryLimitMb).HasDefaultValue(256);
            entity.Property(e => e.DiskLimitMb).HasDefaultValue(256);
            entity.Property(e => e.CreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Tasks)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Creator)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.MoodleAssignmentId).IsUnique();
        });

        modelBuilder.Entity<TestCase>(entity =>
        {
            entity.ToTable("TestCases");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Input).IsRequired();
            entity.Property(e => e.ExpectedOutput).IsRequired();
            entity.Property(e => e.IsPublic).HasDefaultValue(false);
            entity.Property(e => e.Points).HasPrecision(5, 2).HasDefaultValue(1.00m);
            entity.Property(e => e.ExecutionOrder).HasDefaultValue(1);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Task)
                .WithMany(t => t.TestCases)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReferenceSolution>(entity =>
        {
            entity.ToTable("ReferenceSolutions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SourceCode).IsRequired();
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsValid).HasDefaultValue(false);

            entity.HasOne(e => e.Task)
                .WithMany(t => t.ReferenceSolutions)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Uploader)
                .WithMany(u => u.ReferenceSolutions)
                .HasForeignKey(e => e.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AdditionalFile>(entity =>
        {
            entity.ToTable("AdditionalFiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Filename).HasMaxLength(255).IsRequired();
            entity.Property(e => e.FileContent).IsRequired();
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Task)
                .WithMany(t => t.AdditionalFiles)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.ToTable("Submissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubmissionTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.FinalGrade).HasPrecision(5, 2);

            entity.HasOne(e => e.Task)
                .WithMany(t => t.Submissions)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.ToTable("TestResults");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(255).IsRequired();
            entity.Property(e => e.DiskUsedMb).HasPrecision(10, 2);
            entity.Property(e => e.Judge0Token).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.TestCase)
                .WithMany(tc => tc.TestResults)
                .HasForeignKey(e => e.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Submission)
                .WithMany(s => s.TestResults)
                .HasForeignKey(e => e.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}