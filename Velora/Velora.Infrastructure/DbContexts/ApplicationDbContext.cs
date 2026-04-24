using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Velora.Domain.Model;
using Velora.Domain.ModelAggregate.Sales;
using Velora.Domain.ModelAggregate.User;
using Velora.Infrastructure.ModelConfiguration;
using Velora.Infrastructure.ModelConfiguration.Sales;

namespace Velora.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, UserRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<UserRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<Guid>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<Guid>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<Guid>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<Guid>>(entity => entity.ToTable("UserTokens"));

            builder.ApplyConfiguration(new BrandConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
        }

        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<Brand> Brand { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }
    }
}
