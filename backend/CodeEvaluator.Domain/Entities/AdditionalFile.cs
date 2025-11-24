namespace CodeEvaluator.Domain.Entities;

public class AdditionalFile
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Filename { get; set; } = string.Empty;
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
    public int FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public Task Task { get; set; } = null!;
}