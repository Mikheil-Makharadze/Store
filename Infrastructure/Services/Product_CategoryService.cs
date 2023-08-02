using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class Product_CategoryService : IProduct_CategoryService
    {
        private readonly AppDbContext Context;

        public Product_CategoryService(AppDbContext context) 
        {
            Context = context;
        }

        public async Task CreateProduct_Category(int productId, int categoryId)
        {
            var product_category = new Product_Category()
            {
                ProductId = productId,
                CategoryId = categoryId
            };
            
            await Context.Product_Categories.AddAsync(product_category);
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryId(int Id)
        {
            return await Context.Product_Categories.Where(n => n.CategoryId == Id).Select(n => n.Product).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetByProductId(int Id)
        {
            return await Context.Product_Categories.Where(n => n.ProductId == Id).Select(n => n.Category).ToListAsync();
        }

    }
}
