using System.Net.Http.Json;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using System.Text.Json.Serialization;

namespace CodeEvaluator.Application.Services
{
    public class MoodleAuthService : IMoodleAuthService
    {
        private readonly HttpClient _http;

        private const string MoodleUrl = "http://localhost:8000";

        private const string ServiceName = "moodle_mobile_app"; //промени тук ако еxternal service-ти се казва по друг начин

        public MoodleAuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<MoodleAuthResult> AuthenticateAsync(
            string username,
            string password)
        {
            Console.WriteLine("MAMA MU DEEEBA");
            var loginUrl = $"{MoodleUrl.TrimEnd('/')}/login/token.php" +
               $"?username={Uri.EscapeDataString(username)}" +
               $"&password={Uri.EscapeDataString(password)}" +
               $"&service={Uri.EscapeDataString(ServiceName)}";
               Console.WriteLine("MAMA MU DEEEBA");

               Console.WriteLine($"[DEBUG] C# is calling: {loginUrl}");

            var response = await _http.PostAsync(loginUrl, null);

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
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    private class SiteInfoResponse
    {
        [JsonPropertyName("userid")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = "";
    }

    }

}
