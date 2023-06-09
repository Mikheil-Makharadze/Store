using Web.Models.DTO;
using Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Services.Interfaces
{
    public interface IGenericService<T,TCreate>
    {
        Task<List<T>> GetAllAsync(string token);
        Task<T> GetByIdAsync(int id, string token);
        Task AddAsync(object entity, string token);
        Task UpdateAsync(int id, object entity, string token);
        Task DeleteAsync(int id, string token);
    }
}
