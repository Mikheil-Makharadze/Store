using API.DTO;
using API.DTO.IdentityDTO;
using API.Response;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("User")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser(Email email)
        {
            var user = await userManager.FindByEmailAsync(email.email);

            if (user == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User was not found" }
                });
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var usersInfo = new UserWithRoleDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = userRoles.FirstOrDefault()
            };

            return Ok( new APIResponse { Result = usersInfo });
        }

        [HttpGet("Users")]
        public async Task<ActionResult<APIResponse>> GetAllUsers(SearchString searchString)
        {
            var users = await userManager.Users.ToListAsync();

            var usersInfo = new List<UserWithRoleDTO>();

            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var userDTO = new UserWithRoleDTO
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault()
                };

                usersInfo.Add(userDTO);
            }

            if (searchString.Search != null)
            {
                var search = searchString.Search.Trim();
                usersInfo = usersInfo.Where(n => n.Email.Contains(search) || n.Role.Contains(search)).ToList();
            }

            return Ok( new APIResponse { Result = usersInfo });
        }        

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ChangeRole(UserWithRoleDTO userUpdate)
        {
            var user = await userManager.FindByEmailAsync(userUpdate.Email);

            if (user == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found" }
                });
            }

            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles);

            await userManager.AddToRoleAsync(user, userUpdate.Role);

            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Remove(Email email)
        {
            var user = await userManager.FindByEmailAsync(email.email);

            if (user == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found" }
                });
            }

            await userManager.DeleteAsync(user);

            return Ok( new APIResponse { Result = email.email });
        }
    }
}
