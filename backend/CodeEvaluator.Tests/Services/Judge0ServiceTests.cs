using Xunit;
using Moq;
using CodeEvaluator.Application.Services;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Judge0.Client;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeEvaluator.Tests.Services
{
    public class Judge0ServiceTests
    {
        [Fact]
        public async Task ExecuteCodeAsync_ValidCode_ReturnsToken()
        {
            var mockClient = new Mock<Judge0Client>();

            mockClient
                .Setup(c => c.SendSubmissionAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\"token\":\"abc123\"}");

            var service = new Judge0Service(mockClient.Object);

            var submission = new Judge0SubmissionDTO
            {
                SourceCode = "Console.WriteLine(\"Hello\");",
                LanguageId = 51
            };

            var result = await service.ExecuteCodeAsync(submission);

            Assert.NotNull(result);
            Assert.Equal("abc123", result.Token);
        }

        [Fact]
        public async Task ExecuteBatchCodeAsync_MultipleSubmissions_ReturnsTokensList()
        {
            var mockClient = new Mock<Judge0Client>();

            mockClient
                .Setup(c => c.SendSubmissionBatchAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("[{\"token\":\"token1\"},{\"token\":\"token2\"}]");

            var service = new Judge0Service(mockClient.Object);

            var submissions = new List<Judge0SubmissionDTO>
            {
                new() { SourceCode = "Code1", LanguageId = 51 },
                new() { SourceCode = "Code2", LanguageId = 51 }
            };

            var tokens = await service.ExecuteBatchCodeAsync(submissions);

            Assert.Equal(2, tokens.Count);
            Assert.Contains("token1", tokens);
            Assert.Contains("token2", tokens);
        }
    }
}
