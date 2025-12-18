using System.Text;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Domain.Entities;
using Newtonsoft.Json;
using CodeEvaluator.Judge0.Client;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;


namespace CodeEvaluator.Application.Services
{
    public class Judge0Service : IJudge0Service
    {

        private readonly Judge0Client _judge0Client;
        public Judge0Service(Judge0Client client)
        {
          _judge0Client = client;
          Judge0Config.SetBaseUrl("http://172.21.229.127:2358");
        }

        public async Task<Judge0ResultDto> ExecuteCodeAsync(Judge0SubmissionDTO submission)
        {
            var payload = new
            {
                source_code = Utils.ToBase64(submission.SourceCode),
                language_id = submission.LanguageId,
                cpu_time_limit = submission.CpuTimeLimit,
                memory_limit = submission.MemoryLimit,
                stack_limit = submission.StackLimit,
                disk_limit = submission.MaxFileSize,
                stdin = submission.StdIn,
                expected_output = submission.ExpectedOutput,
                additional_files = submission.AdditionalFiles
                
            };
            

            var json = JsonConvert.SerializeObject(payload);
            var response = await _judge0Client.SendSubmissionAsync(json);

           return JsonConvert.DeserializeObject<Judge0ResultDto>(response)!;
        }
        public async Task<List<String>> ExecuteBatchCodeAsync(List<Judge0SubmissionDTO> submissions)
        {
            var payload = new
            {
                submissions = submissions.ConvertAll(submission => new
                {
                    source_code = Utils.ToBase64(submission.SourceCode),
                    language_id = submission.LanguageId,
                    cpu_time_limit = submission.CpuTimeLimit,
                    memory_limit = submission.MemoryLimit,
                    stack_limit = submission.StackLimit,
                    disk_limit = submission.MaxFileSize,
                    stdin = submission.StdIn,
                    expected_output = submission.ExpectedOutput,
                    additional_files = submission.AdditionalFiles
                })
            };

            var json = JsonConvert.SerializeObject(payload);
            var response = await _judge0Client.SendSubmissionBatchAsync(json);
            Console.WriteLine(response);
            List<String> tokens = new List<string>();
            if(JsonDocument.Parse(response).RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in JsonDocument.Parse(response).RootElement.EnumerateArray())
                {
                    tokens.Add(element.GetProperty("token").GetString()!);
                }
                
            }
          
            return tokens;
        }
        public async Task<Judge0ResultDto> GetSubmissionResponseDtoFromJudge0(string token)
        {
            var response = await _judge0Client.GetSubmissionByTokenAsync(token);
            return JsonConvert.DeserializeObject<Judge0ResultDto>(response);
        }

        public async Task<List<Judge0ResultDto>> GetSubbmisionBatchAsync(List<string> tokens)
        {
            Console.WriteLine("\n Batch get is getting "+string.Join(",",tokens) + "\n");
            var response = await _judge0Client.GetSubmissionByTokensAsync(string.Join(",",tokens));
             var root = JsonConvert.DeserializeObject<JObject>(response);
             
              var submissions = root["submissions"]!
                .ToObject<List<Judge0ResultDto>>();

        Console.WriteLine("\nBatch get is returning "+submissions.Count);
        return submissions;

        }
    }
}
