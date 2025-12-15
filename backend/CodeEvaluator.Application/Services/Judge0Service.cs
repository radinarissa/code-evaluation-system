using System.Text;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Domain.Entities;
using Newtonsoft.Json;
using CodeEvaluator.Judge0.Client;
using System.Threading.Tasks;


namespace CodeEvaluator.Application.Services
{
    public class Judge0Service : IJudge0Service
    {

        private readonly Judge0Client _judge0Client;
        public Judge0Service(Judge0Client client)
        {
          _judge0Client = client;
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
                stdin = submission.StdIn,
                expected_output = submission.ExpectedOutput
            };
            

            var json = JsonConvert.SerializeObject(payload);
            var response = await _judge0Client.SendSubmissionAsync(json);

           return JsonConvert.DeserializeObject<Judge0ResultDto>(response)!;
        }
        public async Task<Judge0SubmissionDTO> GetSubmissionResponseDtoFromJudge0(string token)
        {
            var response = await _judge0Client.GetSubmissionByTokenAsync(token);
            return JsonConvert.DeserializeObject<Judge0SubmissionDTO>(response)!;
        }
    }
}
