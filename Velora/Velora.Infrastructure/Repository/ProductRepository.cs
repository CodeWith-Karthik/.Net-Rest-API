using Microsoft.EntityFrameworkCore;
using Velora.Domain.Contracts;
using Velora.Domain.Model;
using Velora.Infrastructure.DbContexts;

namespace Velora.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetByIdsAsync(List<int> ids)
        {
            return await _dbContext.Product.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task UpdateRangeAsync(List<Product> products, bool instantSave = true)
        {
            _dbContext.Product.UpdateRange(products);

            if (instantSave) await _dbContext.SaveChangesAsync();
        }

        //public async Task<Product> GetByIdWithIncludeAsync(int id)
        //{
        //    return await _dbContext.Product.Include(x => x.Brand).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        //}

        //public async Task<List<Product>> GetWithIncludeAsync()
        //{
        //    return await _dbContext.Product.Include(x => x.Brand).Include(x => x.Category).ToListAsync();
        //}
    }
}
