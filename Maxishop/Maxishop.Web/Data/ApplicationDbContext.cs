using Maxishop.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace Maxishop.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }
    }
}
