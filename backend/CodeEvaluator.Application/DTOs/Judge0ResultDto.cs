using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeEvaluator.Application.DTOs
{
    public class Judge0ResultDto
    {
        [JsonPropertyName("stdin")]
        public string Stdin { get; set; }
        [JsonPropertyName("stdout")]
        public string Stdout { get; set; }
        [JsonPropertyName("stderr")]
        public string Stderr { get; set; }
        [JsonPropertyName("compile_output")]
        public string CompileOutput { get; set; }
        [JsonPropertyName("status")]
        public Judge0Status Status { get; set; }
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("memory")]
        public long? Memory { get; set; }
        public int FileSize { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

    }

    public class Judge0Status
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

}
