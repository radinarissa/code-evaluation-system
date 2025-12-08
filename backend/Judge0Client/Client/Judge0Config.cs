

public static class Judge0Config
{
    public static string BaseUrl {get; set;}
    public static void SetBaseUrl(string baseUrl)
    {
        if(string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException("BaseUrl cannot be null or empty", nameof(baseUrl));
        }

        if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("BaseUrl must include http:// or https:// scheme", nameof(baseUrl));
        }

        BaseUrl = baseUrl.TrimEnd('/');
    }
}