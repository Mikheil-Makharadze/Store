using Web.Models;
using Web.Models.DTO.IdentityDTO;

namespace Web.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserWithRoleDTO>> GetAllUsers(string? searchString, string token);
        Task<UserWithRoleDTO> GetUser(string email, string token);
        Task ChangeRole(UserWithRoleDTO Userinfo, string token);
        Task DeleteUser(string email, string token);
    }
}
