using Core.Entities;
using Core.Interfaces;
using Infrastructure.CustomeException;
using Infrastructure.Data.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext Context;
        internal DbSet<T> dbSet;

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            dbSet = Context.Set<T>();
        }
        public virtual async Task<int> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
            return entity.Id;
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            dbSet.Remove(entity);
            await SaveAsync();
            return entity.Id;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }


        public virtual async Task<T> GetByIdAsync(int id) => await dbSet.FirstOrDefaultAsync(n => n.Id == id) ?? throw new NotFoundException($"{dbSet.EntityType} with Id: {id} was not found");

        public virtual async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.FirstOrDefaultAsync(n => n.Id == id) ?? throw new NotFoundException($"{dbSet.EntityType} with Id: {id} was not found"); ;
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            var previousEntity = await GetByIdAsync(entity.Id);
            dbSet.Update(entity);
            await SaveAsync();
            return entity.Id;
        }

        protected async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
