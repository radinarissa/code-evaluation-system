using System.Text.Json.Serialization;

namespace CodeEvaluator.Judge0.Client

{
public class Judge0Submission
{
    [JsonPropertyName("source_code")]
    public string SourceCode { get; set; }
    [JsonPropertyName("language_id")]
    public int LanguageId { get; set; }
    [JsonPropertyName("cpu_time_limit")]
    public int? CpuTimeLimit { get; set; }
    [JsonPropertyName("memory_limit")]
    public int? MemoryLimit { get; set; }
    [JsonPropertyName("stack_limit")]
    public int? StackLimit { get; set; }
    [JsonPropertyName("stdin")]
    public string? StdIn { get; set; }
    [JsonPropertyName("expected_output")]
    public string? ExpectedOutput { get; set; }
}
}