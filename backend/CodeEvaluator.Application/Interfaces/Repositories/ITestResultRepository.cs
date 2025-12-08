using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface ITestResultRepository : IRepository<TestResult>
{
    Task<IEnumerable<TestResult>> GetResultsBySubmissionIdAsync(int submissionId);
    Task<IEnumerable<TestResult>> GetResultsByTestCaseIdAsync(int testCaseId);
}