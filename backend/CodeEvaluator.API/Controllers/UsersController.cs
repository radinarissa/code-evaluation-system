using CodeEvaluator.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeEvaluator.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public UsersController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? role = null)
    {
        var q = _db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(role))
            q = q.Where(u => u.Role == role);

        var users = await q
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Select(u => new {
                id = u.Id,
                moodleId = u.MoodleId,
                username = u.Username,
                email = u.Email,
                firstName = u.FirstName,
                lastName = u.LastName,
                role = u.Role,
                createdAt = u.CreatedAt,
                updatedAt = u.UpdatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var u = await _db.Users.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new {
                id = x.Id,
                moodleId = x.MoodleId,
                username = x.Username,
                email = x.Email,
                firstName = x.FirstName,
                lastName = x.LastName,
                role = x.Role,
                createdAt = x.CreatedAt,
                updatedAt = x.UpdatedAt
            })
            .SingleOrDefaultAsync();

        return u == null ? NotFound() : Ok(u);
    }
}