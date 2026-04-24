using Maxishop.Web.Data;
using Maxishop.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace Maxishop.Web.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _dbContext.Product.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await _dbContext.Product.AnyAsync(x => x.Id == id);
        }
    }
}
