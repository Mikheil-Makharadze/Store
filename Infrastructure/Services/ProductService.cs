using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : GenericRepository<Product>, IProductService
    {
        private readonly AppDbContext Context;
        public ProductService(AppDbContext context) : base(context)
        {
            Context = context;
        }

        //public async Task AddAsync(Product product, ICollection<int> categorysId)
        //{
        //    await Context.Product.AddAsync(product);
        //    await SaveAsync();

        //    foreach (var categoryId in categorysId)
        //    {
        //        await Context.Product_Categories.AddAsync(new Product_Category
        //        {
        //            ProductId = product.Id,
        //            CategoryId = categoryId
        //        });
        //    }

        //    await SaveAsync();
        //}

        //public async Task UpdateAsync(Product product, ICollection<int> categorysId)
        //{
        //    Context.Product.Update(product);
        //    await SaveAsync();

        //    var product_Categories = await Context.Product_Categories.Where(n => n.ProductId == product.Id).ToListAsync();
        //    Context.Product_Categories.RemoveRange(product_Categories);
        //    await SaveAsync();

        //    foreach (var categoryId in categorysId)
        //    {
        //        await Context.Product_Categories.AddAsync(new Product_Category
        //        {
        //            ProductId = product.Id,
        //            CategoryId = categoryId
        //        });
        //    }
        //    await SaveAsync();
        //}
        public async Task<IEnumerable<Product>> GetAllDetailsAsync()
        {
            return await Context.Product.AsNoTracking()
                  .AsQueryable().Include(n => n.Producer)
                  .Include(n => n.Product_Categories)
                  .ThenInclude(n => n.Category).ToListAsync();
        }

        public async Task<Product> GetbyIdDetailsAsync(int Id)
        {
            return await Context.Product.AsNoTracking()
                  .AsQueryable().Where(n => n.Id == Id).Include(n => n.Producer)
                  .Include(n => n.Product_Categories)
                  .ThenInclude(n => n.Category).FirstOrDefaultAsync();
        }
    }
}
