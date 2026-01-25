
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

namespace CodeEvaluator.Judge0.Client
{
public class Judge0Client
{
    // Reuse a single static HttpClient instance.
    //public static string BaseUrl {get; set;}

    public Judge0Client()
    {
    }

    private static readonly HttpClient httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    // public static void SetBaseUrl(string baseUrl)
    // {
    //     if(string.IsNullOrWhiteSpace(baseUrl))
    //     {
    //         throw new ArgumentException("BaseUrl cannot be null or empty", nameof(baseUrl));
    //     }

    //     if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
    //         !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
    //     {
    //         throw new ArgumentException("BaseUrl must include http:// or https:// scheme", nameof(baseUrl));
    //     }

    //     BaseUrl = baseUrl.TrimEnd('/');
    // }



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

    public async Task<string> SendSubmissionAsync(string jsonSubmission, CancellationToken ct = default)
    {
        var url = BuildUrl("submissions", "wait=false", "base64_encoded=true", "fields=*");
        using var content = new StringContent(jsonSubmission ?? throw new ArgumentNullException(nameof(jsonSubmission)), Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, content, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        Console.WriteLine("POSTING TO JUDGE0 URL: " + url);
        return body;
    }

    public async Task<string> SendSubmissionBatchAsync(string submissionsJson, CancellationToken ct = default)
    {
        var url = BuildUrl("submissions/batch","wait=false", "base64_encoded=true", "fields=*");

        Console.WriteLine($"[Judge0Client] POSTing to: {url}");

        using var content = new StringContent(submissionsJson ?? throw new ArgumentNullException(nameof(submissionsJson)), Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, content, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        return body;
    }

    public async Task<string> GetSubmissionByTokenAsync(string token, CancellationToken ct = default)
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

    public async Task<string> GetSubmissionByTokensAsync(string tokensCsv, CancellationToken ct = default)
    {
        Console.WriteLine("\nJudge0client is getting: "+tokensCsv);
        // tokensCsv should be comma-separated tokens; we URL encode the full value.
        if (tokensCsv == null) throw new ArgumentNullException(nameof(tokensCsv));
        var encoded = Uri.EscapeDataString(tokensCsv);
        var url = BuildUrl("submissions/batch", $"tokens={encoded}", "base64_encoded=true", "fields=*");

        using var response = await httpClient.GetAsync(url, ct).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Judge0 returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

        Console.WriteLine("Judge0Client is returning"+body);
        return body;
    }
}
}