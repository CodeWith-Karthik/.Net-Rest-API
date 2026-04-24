using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Velora.Domain.Contracts;
using Velora.Domain.Model;
using Velora.Infrastructure.DbContexts;

namespace Velora.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> GetAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id, params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<T> CreateAsync(T entity, bool instantSave = true)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.ModifiedOn = DateTime.UtcNow;

            await _dbContext.Set<T>().AddAsync(entity);

            if (instantSave) await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity, bool instantSave = true)
        {
            entity.ModifiedOn = DateTime.UtcNow;

            _dbContext.Set<T>().Update(entity);

            if (instantSave) await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsRecordExists(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(x => x.Id == id);
        }
    }
}
