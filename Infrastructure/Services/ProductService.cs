using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DB;

namespace Infrastructure.Services
{
    public class ProductService : GenericRepository<Product>, IProductService
    {
        private readonly AppDbContext Context;
        public ProductService(AppDbContext context) : base(context)
        {
            Context = context;
        }
    }
}
