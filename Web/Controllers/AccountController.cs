using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Web.Models;
using Web.Services.Interfaces;
using Web.Models.DTO.IdentityDTO;
using System.IdentityModel.Tokens.Jwt;
using Web.ExceptionFilter.Exceptions;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService authService;

        public AccountController(IAccountService _authService)
        {
            authService = _authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {;
            LoginDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO model = await authService.LoginAsync(loginDTO);

                    var principal = ClaimsIdentity(model);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString(SD.SessionToken, model.Token);

                    return RedirectToAction("Index", "Game");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(loginDTO);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await authService.RegisterAsync(obj);

                    return RedirectToAction("Login");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(obj);

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            SignOut("Cookies", "oidc");
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Game");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private ClaimsPrincipal ClaimsIdentity(UserDTO model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Email, jwt.Claims.FirstOrDefault(u => u.Type == "email").Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            return new ClaimsPrincipal(identity);
        }
    }
}
