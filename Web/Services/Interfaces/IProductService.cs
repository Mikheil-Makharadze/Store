using System.Linq.Expressions;
using Web.Models;
using Web.Models.DTO;

namespace Web.Services.Interfaces
{
    public interface IProductService : IGenericService<ProductDTO, ProductCreateDTO>
    {
        Task<List<ProductDTO>> GetAllDetailsAsync(string token);
    }
}
