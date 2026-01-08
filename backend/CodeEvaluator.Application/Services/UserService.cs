using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using CodeEvaluator.Domain.Entities;
using CodeEvaluator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeEvaluator.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        public UserService(ApplicationDbContext db) => _db = db;

        public async Task<User> UpsertFromMoodleAsync(MoodleUserDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.MoodleId == dto.MoodleId);

            if (user == null)
            {
                user = new User
                {
                    MoodleId = dto.MoodleId,
                    Username = dto.Username,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Role = dto.Role,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                _db.Users.Add(user);
            }
            else
            {
                user.Username = dto.Username;
                user.Email = dto.Email;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Role = dto.Role;
                user.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return user;
        }
    }
}