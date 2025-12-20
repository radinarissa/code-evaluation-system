using System.Security.Claims;
using CodeEvaluator.Application.DTOs;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class ApiAuthController : ControllerBase
    {
        private readonly IMoodleAuthService _moodleAuth;

        public ApiAuthController(IMoodleAuthService moodleAuth)
        {
            _moodleAuth = moodleAuth;
        }

        /// <summary>
        /// Authenticates a user via Moodle and returns user data.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _moodleAuth.AuthenticateAsync(request.Username, request.Password);

            if (!result.Success)
            {
                return Unauthorized(new LoginResponseDto
                {
                    Success = false,
                    Error = result.Error ?? "Invalid credentials"
                });
            }

            var userId = await _moodleAuth.GetUserIdAsync(result.Token!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim("MoodleUserId", userId.ToString()),
                new Claim("MoodleToken", result.Token!)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return Ok(new LoginResponseDto
            {
                Success = true,
                User = new UserDto
                {
                    Id = userId,
                    Username = request.Username,
                    FirstName = request.Username, // Moodle doesn't return these by default
                    LastName = "",
                    Role = "Teacher"
                }
            });
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok(new { success = true, message = "Logged out successfully" });
        }

        /// <summary>
        /// Gets the current authenticated user.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Unauthorized(new { success = false, error = "Not authenticated" });
            }

            var username = User.Identity.Name;
            var userIdClaim = User.FindFirst("MoodleUserId");
            var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            return Ok(new UserDto
            {
                Id = userId,
                Username = username ?? "",
                FirstName = username ?? "",
                LastName = "",
                Role = "Teacher"
            });
        }
    }
}
