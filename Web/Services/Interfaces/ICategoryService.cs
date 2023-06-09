using Web.Models.DTO;
using Web.Models;

namespace Web.Services.Interfaces
{
    public interface ICategoryService : IGenericService<CategoryDTO, CategoryCreateDTO>
    {
        Task<List<CategoryDTO>> GetAllDetailsAsync(string token);
    }
}
