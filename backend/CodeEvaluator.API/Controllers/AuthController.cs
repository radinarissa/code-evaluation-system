using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMoodleAuthService _moodleAuth;

    public AuthController(IMoodleAuthService moodleAuth)
    {
        _moodleAuth = moodleAuth;
    }

    [HttpPost("moodle/login")]
    public async Task<IActionResult> MoodleLogin([FromBody] LoginRequest request)
    {
        var result = await _moodleAuth.AuthenticateAsync(request.Username, request.Password);

        if (!result.Success)
        {
            return Unauthorized(new { message = result.Error });
        }

        var userId = await _moodleAuth.GetUserIdAsync(result.Token!);

        Response.Cookies.Append("moodleUserId", userId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return Ok(new
        {
            username = request.Username,
            firstName = "User",
            lastName = "Name",
            role = "Teacher",
            moodleId = userId
        });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}