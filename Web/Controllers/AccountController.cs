using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Web.Models;
using Web.Services.Interfaces;
using Web.Models.DTO.IdentityDTO;
using System.IdentityModel.Tokens.Jwt;

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
            APIResponse response = await authService.LoginAsync(loginDTO);
            if (response != null && response.IsSuccess)
            {
                UserDTO model = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
                
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Email, jwt.Claims.FirstOrDefault(u => u.Type == "email").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(loginDTO);
            }
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
            APIResponse result = await authService.RegisterAsync(obj);
            if (result != null && result.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            SignOut("Cookies", "oidc");
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Product");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
