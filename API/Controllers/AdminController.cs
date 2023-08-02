using API.DTO;
using API.DTO.IdentityDTO;
using API.Response;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Services;
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
        private readonly ITokenService tokenService;

        public AdminController(UserManager<User> _userManager, SignInManager<User> _signInManager, ITokenService _tokenService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _tokenService;
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

            return Ok( new APIResponse { Result = await tokenService.CreateToken(user) });
        }

        [HttpGet("Users")]
        public async Task<ActionResult<APIResponse>> GetAllUsers(SearchString searchString)
        {
            var users = await userManager.Users.ToListAsync();

            var usersInfo = new List<string>();

            if (searchString.Search != null)
            {
                var search = searchString.Search.Trim();
                users = users.Where(n => n.Email.Contains(search)).ToList();
            }

            foreach (var user in users)
            {
                usersInfo.Add(await tokenService.CreateToken(user));
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
