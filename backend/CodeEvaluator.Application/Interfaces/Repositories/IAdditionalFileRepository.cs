using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Repositories;

public interface IAdditionalFileRepository : IRepository<AdditionalFile>
{
    Task<IEnumerable<AdditionalFile>> GetFilesByTaskIdAsync(int taskId);
}