using System.Text;
using System.Text.Json;

public static class Utils
{
    public static string ToBase64(string input)
    {
        if (input == null) return "";
        var bytes = Encoding.UTF8.GetBytes(input);
        return System.Convert.ToBase64String(bytes);
    }

   
    public static string FromBase64(string? input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        try
        {
            var bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            // Not a valid base64 string - return original value unchanged.
            return input;
        }
    }

 
    public static string FromBase64(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null) return "";
        if (element.ValueKind != JsonValueKind.String)
        {
            // For non-string value kinds, fall back to raw JSON text
            return element.GetRawText();
        }

        var maybe = element.GetString();
        return FromBase64(maybe);
    }

    public static string ReadableResponse(string base64Response)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Raw response:");
        sb.AppendLine(base64Response);
        sb.AppendLine();

        if (string.IsNullOrWhiteSpace(base64Response))
        {
            sb.AppendLine("Empty response");
            return sb.ToString();
        }

        JsonDocument? doc = null;
        try
        {
            doc = JsonDocument.Parse(base64Response);
        }
        catch (JsonException)
        {
            sb.AppendLine("Response is not valid JSON.");
            return sb.ToString();
        }

        var result = doc.RootElement;

        // Token
        if (result.TryGetProperty("token", out var tokenEl) && tokenEl.ValueKind == JsonValueKind.String)
        {
            var token = tokenEl.GetString();
            if (!string.IsNullOrEmpty(token))
            {
                sb.AppendLine("Token: " + token);
            }
        }

        // Status
        if (result.TryGetProperty("status", out var statusEl) && statusEl.ValueKind == JsonValueKind.Object)
        {
            int sid = -1;
            string sdesc = "";

            if (statusEl.TryGetProperty("id", out var idEl) && idEl.ValueKind == JsonValueKind.Number && idEl.TryGetInt32(out var id))
            {
                sid = id;
            }
            if (statusEl.TryGetProperty("description", out var descEl) && descEl.ValueKind == JsonValueKind.String)
            {
                sdesc = descEl.GetString() ?? "";
            }

            sb.AppendLine($"Status id: {sid} - {sdesc}");
        }

        // Decode fields if present
        string stdout = "";
        string stderr = "";
        string compileOutput = "";

        if (result.TryGetProperty("stdout", out var outEl)) stdout = FromBase64(outEl);
        if (result.TryGetProperty("stderr", out var errEl)) stderr = FromBase64(errEl);
        if (result.TryGetProperty("compile_output", out var compEl)) compileOutput = FromBase64(compEl);

        sb.AppendLine();
        sb.AppendLine("--- RESULT ---");
        sb.AppendLine("STDOUT:");
        sb.AppendLine(stdout);
        sb.AppendLine("STDERR:");
        sb.AppendLine(stderr);
        sb.AppendLine("COMPILE OUTPUT:");
        sb.AppendLine(compileOutput);

        return sb.ToString();
    }
}