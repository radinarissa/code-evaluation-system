using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;

namespace CodeEvaluator.Application.Services
{
    public class MoodleAuthService : IMoodleAuthService
    {
        private readonly HttpClient _http;

        private const string MoodleTokenUrl =
            "https://YOURMOODLE/login/token.php";

        private const string ServiceName = "DotNetAuth";

        public MoodleAuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<MoodleAuthResult> AuthenticateAsync(
            string username,
            string password)
        {
            var response = await _http.PostAsync(
                $"{MoodleTokenUrl}?service={ServiceName}" +
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

        private class MoodleTokenResponse
        {
            public string? Token { get; set; }
            public string? Error { get; set; }
        }
    }

}
