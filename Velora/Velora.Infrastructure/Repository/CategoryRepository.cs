using Velora.Domain.Contracts;
using Velora.Domain.Model;
using Velora.Infrastructure.DbContexts;

namespace Velora.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
