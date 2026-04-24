using Velora.Domain.Contracts;
using Velora.Domain.Model;
using Velora.Infrastructure.DbContexts;

namespace Velora.Infrastructure.Repository
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
