using System.Security.Claims;
using CodeEvaluator.API.Models;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CodeEvaluator.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMoodleAuthService _moodleAuth;

        public AuthController(IMoodleAuthService moodleAuth)
        {
            _moodleAuth = moodleAuth;
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
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
