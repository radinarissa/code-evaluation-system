namespace CodeEvaluator.Domain.Entities;

public class ReferenceSolution
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string SourceCode { get; set; } = string.Empty;
    public int UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool IsValid { get; set; }
    public string? ValidationMessage { get; set; }
    public Task Task { get; set; } = null!;
    public User Uploader { get; set; } = null!;
}