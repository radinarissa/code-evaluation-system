using System.Text.Json.Serialization;
public class Judge0SubmissionDTO
{
    [JsonPropertyName("source_code")]
    public string SourceCode { get; set; }
    [JsonPropertyName("language_id")]
    public int LanguageId { get; set; }
    [JsonPropertyName("cpu_time_limit")]
    public decimal? CpuTimeLimit { get; set; }
    [JsonPropertyName("memory_limit")]
    public int? MemoryLimit { get; set; }
    [JsonPropertyName("stack_limit")]
    public int? StackLimit { get; set; }
    [JsonPropertyName ("max_file_size")]
    public int? MaxFileSize { get; set; }
    [JsonPropertyName("stdin")]
    public string? StdIn { get; set; }
    [JsonPropertyName("expected_output")]
    public string? ExpectedOutput { get; set; }
    [JsonPropertyName("additional_files")]
    public string? AdditionalFiles { get; set; }
}