using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http.Json;

namespace CodeEvaluator.Application.Services
{
    public class MoodleAuthService : IMoodleAuthService
    {
        private readonly string _token;
        private readonly HttpClient _http;

        private const string MoodleUrl = "http://localhost:8000";

        private const string ServiceName = "DotNetAuth"; //промени тук ако еxternal service-ти се казва по друг начин

        public MoodleAuthService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _token = config["Moodle:ServiceToken"]!;
        }

        public async Task<MoodleAuthResult> AuthenticateAsync(
            string username,
            string password)
        {
            var response = await _http.PostAsync(
                $"{MoodleUrl}/login/token.php?service={ServiceName}" +
                $"&username={username}&password={password}",
                null);

            if (!response.IsSuccessStatusCode)
            {
                return new MoodleAuthResult
                {
                    Success = false,
                    Error = "Unable to contact Moodle"
                };
            }

            var json = await response.Content.ReadFromJsonAsync<MoodleTokenResponse>();

            if (json?.Token == null)
            {
                return new MoodleAuthResult
                {
                    Success = false,
                    Error = json?.Error ?? "Invalid login"
                };
            }

            return new MoodleAuthResult
            {
                Success = true,
                Token = json.Token
            };
        }

        public async Task<int> GetUserIdAsync(string token)
        {
            // 1️⃣ Get user ID
            var siteInfo = await _http.GetFromJsonAsync<SiteInfoResponse>(
                $"{MoodleUrl}/webservice/rest/server.php" +
                $"?wstoken={token}" +
                $"&wsfunction=core_webservice_get_site_info" +
                $"&moodlewsrestformat=json");

            return siteInfo?.UserId ?? 0;
        }

        
        public async Task<bool> SendGradeAsync(
            int assignmentId,
            int studentUserId,
            decimal grade,
            string feedbackHtml)
        {
            var values = new Dictionary<string, string>
            {
                ["wstoken"] = _token,
                ["wsfunction"] = "mod_assign_save_grade",
                ["moodlewsrestformat"] = "json",

                ["assignmentid"] = assignmentId.ToString(),
                ["userid"] = studentUserId.ToString(),
                ["grade"] = grade.ToString(CultureInfo.InvariantCulture),
                ["attemptnumber"] = "-1",
                ["addattempt"] = "0",
                ["workflowstate"] = "graded",

                ["plugindata[assignfeedback_comments][commenttext]"] = feedbackHtml,
                ["plugindata[assignfeedback_comments][commentformat]"] = "1"
            };

            var content = new FormUrlEncodedContent(values);

            var response = await _http.PostAsync(
                $"{MoodleUrl}/webservice/rest/server.php",
                content);

            var body = await response.Content.ReadAsStringAsync();

            // Moodle returns {} on success
            return response.IsSuccessStatusCode && body.Trim() == "{}";
        }

        private class MoodleTokenResponse
        {
            public string? Token { get; set; }
            public string? Error { get; set; }
        }

        private class SiteInfoResponse
        {
            public int UserId { get; set; }
            public string Username { get; set; } = "";
        }

    }

}
