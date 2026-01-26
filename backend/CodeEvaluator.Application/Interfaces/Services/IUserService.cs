using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> UpsertFromMoodleAsync(MoodleUserDto dto);
    }
}