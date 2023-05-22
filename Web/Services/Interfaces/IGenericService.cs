using Web.Models.DTO;
using Web.Models;

namespace Web.Services.Interfaces
{
    public interface IGenericService
    {
        Task<APIResponse> GetAllAsync(string token);
        Task<APIResponse> GetByIdAsync(int id, string token);
        Task<APIResponse> AddAsync(object entity, string token);
        Task<APIResponse> UpdateAsync(object entity, string token);
        Task<APIResponse> DeleteAsync(int id, string token);
    }
}
