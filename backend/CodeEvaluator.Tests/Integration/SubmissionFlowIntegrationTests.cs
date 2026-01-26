using Xunit;
using Moq;
using CodeEvaluator.Application.Services;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Infrastructure.Data;
using CodeEvaluator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeEvaluator.Tests.Integration
{
    public class SubmissionFlowIntegrationTests
    {
        [Fact]
        public async System.Threading.Tasks.Task CreateSubmission_WithValidData_CreatesSubmissionAndTestResults()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var user = new User
            {
                Id = 1,
                MoodleId = 1001,
                Username = "testuser",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                Role = "Student"
            };
            context.Users.Add(user);

            var task = new CodeEvaluator.Domain.Entities.Task
            {
                Id = 1,
                Title = "Test Task",
                Description = "Test",
                MaxPoints = 100,
                TimeLimitS = 5,
                MemoryLimitKb = 256000,
                StackLimitKb = 128000,
                DiskLimitKb = 512000,
                CreatedBy = 1,
                MoodleCourseId = 1,
                CreationDate = DateTime.UtcNow
            };
            context.Tasks.Add(task);

            var testCase1 = new TestCase
            {
                Id = 1,
                TaskId = 1,
                Name = "Test 1",
                Input = "5",
                ExpectedOutput = "10",
                IsPublic = true,
                Points = 50
            };

            var testCase2 = new TestCase
            {
                Id = 2,
                TaskId = 1,
                Name = "Test 2",
                Input = "10",
                ExpectedOutput = "20",
                IsPublic = false,
                Points = 50
            };

            context.TestCases.AddRange(testCase1, testCase2);
            await context.SaveChangesAsync();

            var mockJudge0 = new Mock<IJudge0Service>();
            mockJudge0
                .Setup(j => j.ExecuteBatchCodeAsync(It.IsAny<List<Judge0SubmissionDTO>>()))
                .ReturnsAsync(new List<string> { "token1", "token2" });

            var service = new SubmissionService(context, mockJudge0.Object);

            var dto = new SubmissionRequestDto
            {
                TaskId = 1,
                MoodleUserId = 1,
                SourceCode = "int x = int.Parse(Console.ReadLine()); Console.WriteLine(x * 2);",
                Language = 51,
                MoodleSubmissionId = 1001,
                MoodleAttemptId = 1
            };

            // Act
            var submission = await service.CreateSubmissionAndRunJudge0Async(dto);

            // Assert
            Assert.NotNull(submission);
            Assert.Equal(1, submission.UserId);
            Assert.Equal(1, submission.TaskId);
            Assert.Equal("Pending", submission.Status);

            var testResults = context.TestResults.Where(tr => tr.SubmissionId == submission.Id).ToList();
            Assert.Equal(2, testResults.Count);
            Assert.All(testResults, tr => Assert.Equal("Pending", tr.Status));
            Assert.All(testResults, tr => Assert.NotNull(tr.Judge0Token));
        }
    }
}