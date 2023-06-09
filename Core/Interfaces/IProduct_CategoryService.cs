using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProduct_CategoryService 
    {
        Task<IEnumerable<Product>> GetByCategoryId(int Id);
        Task<IEnumerable<Category>> GetByProductId(int Id);
        Task CreateProduct_Category(int productId, int categoryId);
        Task RemoveByCategoryId(int Id);
        Task removeByProductId(int Id);
    }
}
