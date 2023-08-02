using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService : IGenericRepository<Product>
    {
        Task<int> UpdateAsync(Product entity, ICollection<int> categoryIds);
    }
}
