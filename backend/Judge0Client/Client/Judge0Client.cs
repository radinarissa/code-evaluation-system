using System.Text;

namespace CodeEvaluator.Judge0.Client
{
    public class Judge0Client
    {
        private readonly HttpClient _http;

        public Judge0Client(HttpClient httpClient)
        {
            _http = httpClient;
        }

        // Helper: build relative URL (BaseAddress is set in Program.cs)
        private static string BuildPath(string path, params string[] queryParts)
        {
            var p = path.TrimStart('/');
            if (queryParts != null && queryParts.Length > 0)
            {
                p += "?" + string.Join("&", queryParts);
            }
            return p;
        }

        public async Task<string> SendSubmissionAsync(string jsonSubmission, CancellationToken ct = default)
        {
            var path = BuildPath("submissions", "wait=false", "base64_encoded=false", "fields=*");
            using var content = new StringContent(
                jsonSubmission ?? throw new ArgumentNullException(nameof(jsonSubmission)),
                Encoding.UTF8,
                "application/json");

            using var response = await _http.PostAsync(path, content, ct).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return body;
        }

        public async Task<string> SendSubmissionBatchAsync(string submissionsJson, CancellationToken ct = default)
        {
            var path = BuildPath("submissions/batch", "base64_encoded=false", "fields=*");
            using var content = new StringContent(
                submissionsJson ?? throw new ArgumentNullException(nameof(submissionsJson)),
                Encoding.UTF8,
                "application/json");

            using var response = await _http.PostAsync(path, content, ct).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return body;
        }

        public async Task<string> GetSubmissionByTokenAsync(string token, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
            var encoded = Uri.EscapeDataString(token);
            var path = BuildPath($"submissions/{encoded}", "base64_encoded=false", "fields=*");

            using var response = await _http.GetAsync(path, ct).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return body;
        }

        public async Task<string> GetSubmissionByTokensAsync(string tokensCsv, CancellationToken ct = default)
        {
            if (tokensCsv == null) throw new ArgumentNullException(nameof(tokensCsv));
            var encoded = Uri.EscapeDataString(tokensCsv);
            var path = BuildPath("submissions/batch", $"tokens={encoded}", "base64_encoded=false", "fields=*");

            using var response = await _http.GetAsync(path, ct).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return body;
        }
    }
}
