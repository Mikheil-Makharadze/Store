using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.DTO.IdentityDTO;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class UserController : Controller
    {
        private readonly IAdminService adminService;
        public UserController(IAdminService _adminService)
        {
            adminService = _adminService;
        }
        public async Task<IActionResult> Index(string? SearchString)
        {
            var Users = await adminService.GetAllUsers(SearchString, GetToken());

            return View(Users);
        }

        public async Task<IActionResult> Edit(string email)
        {
            var product = await adminService.GetUser(email, GetToken());

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserWithRoleDTO user)
        {
            await adminService.ChangeRole(user, GetToken());

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string email)
        {
            var product = await adminService.GetUser(email, GetToken());

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            await adminService.DeleteUser(email, GetToken());

            return RedirectToAction("Index");
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString(SD.SessionToken);
        }

    }
}
