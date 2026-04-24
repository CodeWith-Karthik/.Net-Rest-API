using Microsoft.Extensions.DependencyInjection;
using Velora.Domain.Contracts;
using Velora.Domain.ModelAggregate.User;
using Velora.Infrastructure.DbContexts;
using Velora.Infrastructure.Repository;

namespace Velora.Infrastructure
{
    public static class InfrastructureDependencyContainer
    {
        public static IServiceCollection AddInfrastrutureServices(this IServiceCollection services)
        {

            services.AddIdentityCore<AppUser>()
                    .AddRoles<UserRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
