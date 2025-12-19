using System.Net.Http.Json;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;

namespace CodeEvaluator.Application.Services
{
    public class MoodleAuthService : IMoodleAuthService
    {
        private readonly HttpClient _http;

        private const string MoodleUrl = "http://localhost:8000";

        private const string ServiceName = "dotnettauth"; //промени тук ако еxternal service-ти се казва по друг начин

        public MoodleAuthService(HttpClient http)
        {
            _http = http;
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
