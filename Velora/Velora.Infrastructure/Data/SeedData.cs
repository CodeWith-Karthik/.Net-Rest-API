using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Velora.Domain.Model;
using Velora.Domain.ModelAggregate.User;
using Velora.Infrastructure.DbContexts;

namespace Velora.Infrastructure.Data
{
    public static class SeedData
    {

        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

            var roles = new List<UserRole>
            {
                new UserRole { Id = Guid.NewGuid(), Name="MasterAdmin", NormalizedName="MASTERADMIN"},
                new UserRole { Id = Guid.NewGuid(), Name="Admin", NormalizedName="ADMIN"},
                new UserRole { Id = Guid.NewGuid(), Name="Customer", NormalizedName="CUSTOMER"},
            };

            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            };
        }

        public static async Task SeedDataAsync(ApplicationDbContext _dbContext)
        {
            if (!_dbContext.Category.Any())
            {
                List<Category> categories = new List<Category>
                {
                    new Category { Name="Electronics", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow},
                    new Category { Name="Toys", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow},
                    new Category { Name="Books", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow},
                    new Category { Name="Fashion", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow},
                    new Category { Name="Furniture", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow},
                };

                await _dbContext.Category.AddRangeAsync(categories);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
