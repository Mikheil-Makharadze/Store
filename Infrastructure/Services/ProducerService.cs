using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProducerService : GenericRepository<Producer>, IProducerService
    {
        private readonly AppDbContext _context;
        public ProducerService(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Producer>> GetAllProducersDetails(string? search)
        {
            IQueryable<Producer> query = _context.Producers;

            if(search != null)
            {
                search = search.Trim();
                query = query.Where(n => n.Name!.Contains(search) || n.Description!.Contains(search));
            }

            var result = await query.Include(n => n.Products).ToListAsync();

            return result;
        }
    }
}
