using Xunit;
using CodeEvaluator.Application.Services;
using CodeEvaluator.Infrastructure.Data;
using CodeEvaluator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using CodeEvaluator.Application.Interfaces.Services; 

namespace CodeEvaluator.Tests.Services
{
    public class SubmissionServiceTests
    {
        [Fact]
        public void ConvertSubmissionToDto_ValidSubmission_ReturnsDtoWithCorrectData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new SubmissionService(context, null!);

            var submission = new Submission
            {
                Id = 1,
                TaskId = 10,
                UserId = 5,
                Code = "Console.WriteLine(\"Test\");",
                Status = "Completed",
                FinalGrade = 85.5m,
                Feedback = "Good job!",
                SubmissionTime = DateTime.UtcNow
            };

            // Act
            var dto = service.ConvertSubmissiontoSubmissionResponseDto(submission);

            // Assert
            Assert.Equal(1, dto.Id);
            Assert.Equal(10, dto.TaskId);
            Assert.Equal(5, dto.StudentId);
            Assert.Equal("Completed", dto.Status);
            Assert.Equal(85.5m, dto.Score);
            Assert.Equal("Good job!", dto.Feedback);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteSubmissionAsync_ExistingSubmission_ReturnsSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new SubmissionService(context, null!);

            var submission = new Submission
            {
                Id = 999,
                TaskId = 1,
                UserId = 1,
                Code = "test",
                Status = "Pending",
                SubmissionTime = DateTime.UtcNow
            };

            context.Submissions.Add(submission);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteSubmissionAsync(999);

            // Assert
            Assert.Equal(ISubmissionService.Status.Success, result);
            Assert.Null(await context.Submissions.FindAsync(999));
        }
    }
}