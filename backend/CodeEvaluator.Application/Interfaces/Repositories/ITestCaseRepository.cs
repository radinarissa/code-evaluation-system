using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface ITestCaseRepository : IRepository<TestCase>
{
    Task<IEnumerable<TestCase>> GetTestCasesByTaskIdAsync(int taskId);
    Task<IEnumerable<TestCase>> GetPublicTestCasesByTaskIdAsync(int taskId);
}