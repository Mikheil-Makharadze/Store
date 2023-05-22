using Web.Models;
using Web.Models.DTO.IdentityDTO;

namespace Web.Services.Interfaces
{
    public interface IAccountService
    {
        Task<APIResponse> LoginAsync(LoginDTO loginDto);
        Task<APIResponse> RegisterAsync(RegisterDTO registerDTO);
    }
}
