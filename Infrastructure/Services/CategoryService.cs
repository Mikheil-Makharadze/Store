using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CategoryService : GenericRepository<Category>, ICategoryService
    {
        public CategoryService(AppDbContext context) : base(context)
        {
            
        }
    }
}
