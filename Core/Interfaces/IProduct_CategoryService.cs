using Core.Entities;

namespace Core.Interfaces
{
    public interface IProduct_CategoryService 
    {
        Task<IEnumerable<Product>> GetByCategoryId(int Id);
        Task<IEnumerable<Category>> GetByProductId(int Id);
        Task CreateProduct_Category(int productId, int categoryId);
    }
}
