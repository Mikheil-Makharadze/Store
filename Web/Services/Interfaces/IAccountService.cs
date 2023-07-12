using Microsoft.AspNetCore.Identity;
using Web.Models;
using Web.Models.DTO.IdentityDTO;

namespace Web.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDto);
        Task RegisterAsync(RegisterDTO registerDTO);
    }
}
