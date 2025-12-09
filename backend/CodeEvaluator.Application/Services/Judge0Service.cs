using System.Text;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Domain.Entities;
using Newtonsoft.Json;

namespace CodeEvaluator.Application.Services
{
    internal class Judge0Service : IJudge0Service
    {
        private readonly HttpClient _http;

        public Judge0Service(HttpClient http)
        {
            _http = http;
        }

        public async Task<Judge0ResultDto> ExecuteCodeAsync(Submission submission)
        {
            var payload = new
            {
                source_code = submission.Code,
                language_id = 51,
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Using ?wait=true so Judge0 returns the finished execution results in one request
            var response = await _http.PostAsync("/submissions?wait=true", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Judge0ResultDto>(responseBody)!;
        }
    }
}
