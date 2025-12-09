
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


public class Test
{
     static Test()
    {
        // Set the url where your judge0 is running
         Judge0Config.SetBaseUrl("http://172.20.199.203:2358");
    }
    public static async Task<string> ExampleSingleSubmission()
    {
    var response = await Client.SendSubmissionAsync(JsonSerializer.Serialize(TestObject()));
    Console.WriteLine(response);
    return response;
    }
    public static async Task<string> ExampleGetSubmussion(string token)
    {
     var response = await Client.GetSubmissionByTokenAsync(token);
     Console.WriteLine(Utils.ReadableResponse(response));
     return response;
     
    }
    public static async Task<string> ExampleBatchSubmission()
    {
        Submission[] arr = { TestObject(), TestObject(), TestObject() };
        var batch = new { submissions = arr };
        var response = await Client.SendSubmissionBatchAsync(JsonSerializer.Serialize(batch));
        Console.WriteLine(response);
        return response;
    }
    public static async Task<List<string>> ExampleBatchGetSubmission()
    {
        string[] tokens = {"21f582a3-04af-4afe-8e57-b32de0cb2751","4442f2cd-6bd5-45d1-81d0-90fe0fbf0884","f4d80159-bbf5-4da4-b984-63ead9d00223"};
        var temp = JsonDocument.Parse(await Client.GetSubmissionByTokensAsync(string.Join(",", tokens)));
        var splitjsons = temp.RootElement.GetProperty("submissions").EnumerateArray().ToArray();
        Console.WriteLine("\n\n\n Deserialized objects:" + splitjsons.Length);
        List<string> deserializedstrings = new List<string>();
        foreach (var item in splitjsons)
        {
            Console.WriteLine(Utils.ReadableResponse(item.GetRawText()));
            deserializedstrings.Add(item.GetRawText());
        }

        return deserializedstrings;
    }
    public static Submission TestObject()
    {
        Submission sub = new Submission();
        var a = Encoding.UTF8.GetBytes("print('Hello, World from latest testing')");
        sub.SourceCode = Convert.ToBase64String(a);
        sub.LanguageId = 71; // Python 3
        sub.CpuTimeLimit = 2;
        return sub;
    } 
}