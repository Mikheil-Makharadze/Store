using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductService : GenericRepository<Product>, IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> UpdateAsync(Product entity, ICollection<int> categoryIds)
        {
            var Product = await _context.Product.Where(n => n.Id == entity.Id).Include(n => n.Product_Categories).FirstOrDefaultAsync();

            Product?.Product_Categories.Clear();            

            foreach (var categoryId in categoryIds)
            {
                await _context.Product_Categories.AddAsync(
                    new Product_Category
                    {
                        ProductId = entity.Id,
                        CategoryId = categoryId,
                    });
            }

            return await UpdateAsync(Product!);
        }
    }
}
