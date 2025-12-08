using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public static class Client
{
    // Reuse a single static HttpClient instance.
    private static readonly HttpClient httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    private static string BuildUrl(string path, params string[] queryParts)
    {
        if (string.IsNullOrEmpty(Judge0Config.BaseUrl))
            throw new InvalidOperationException("Judge0Config.BaseUrl is not set. Call Judge0Config.SetBaseUrl(...) at startup.");

        var url = $"{Judge0Config.BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
        if (queryParts != null && queryParts.Length > 0)
        {
            url += "?" + string.Join("&", queryParts);
        }
        return url;
    }

    public static async Task<string> SendSubmissionAsync(string jsonSubmission, CancellationToken ct = default)
    {
        var url = BuildUrl("submissions", "wait=false", "base64_encoded=true", "fields=*");
        using var content = new StringContent(jsonSubmission ?? throw new ArgumentNullException(nameof(jsonSubmission)), Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, content, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        return body;
    }

    public static async Task<string> SendSubmissionBatchAsync(string submissionsJson, CancellationToken ct = default)
    {
        var url = BuildUrl("submissions/batch", "base64_encoded=true", "fields=*");
        using var content = new StringContent(submissionsJson ?? throw new ArgumentNullException(nameof(submissionsJson)), Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, content, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        return body;
    }

    public static async Task<string> GetSubmissionByTokenAsync(string token, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
        var encoded = Uri.EscapeDataString(token);
        var url = BuildUrl($"submissions/{encoded}", "base64_encoded=true", "fields=*");

        using var response = await httpClient.GetAsync(url, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        return body;
    }

    public static async Task<string> GetSubmissionByTokensAsync(string tokensCsv, CancellationToken ct = default)
    {
        // tokensCsv should be comma-separated tokens; we URL encode the full value.
        if (tokensCsv == null) throw new ArgumentNullException(nameof(tokensCsv));
        var encoded = Uri.EscapeDataString(tokensCsv);
        var url = BuildUrl("submissions/batch", $"tokens={encoded}", "base64_encoded=true", "fields=*");

        using var response = await httpClient.GetAsync(url, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        return body;
    }
}