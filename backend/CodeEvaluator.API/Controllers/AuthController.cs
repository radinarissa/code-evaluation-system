using System.Security.Claims;
using CodeEvaluator.API.Models;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CodeEvaluator.Application.DTOs;

namespace CodeEvaluator.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMoodleAuthService _moodleAuth;
        private readonly IUserService _userService;

        public AuthController(IMoodleAuthService moodleAuth, IUserService userService)
        {
            _moodleAuth = moodleAuth;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _moodleAuth.AuthenticateAsync(
                model.Username,
                model.Password);

            if (!result.Success)
            {
                model.Error = result.Error;
                return View(model);
            }

            var userId = await _moodleAuth.GetUserIdAsync(result.Token);

            var backendUser = await _userService.UpsertFromMoodleAsync(new MoodleUserDto
            {
                MoodleId = userId,
                Username = model.Username,
                Email = "",
                FirstName = "",
                LastName = "",
                Role = "Teacher"
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.NameIdentifier, backendUser.Id.ToString()),
                new Claim("MoodleUserId", userId.ToString()),
                new Claim("MoodleToken", result.Token!)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Login");//TODO: redirect to home
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }

}
