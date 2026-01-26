using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface IReferenceSolutionRepository : IRepository<ReferenceSolution>
{
    Task<IEnumerable<ReferenceSolution>> GetSolutionsByTaskIdAsync(int taskId);
    Task<ReferenceSolution?> GetValidSolutionByTaskIdAsync(int taskId);
}