using Microsoft.Extensions.DependencyInjection;
using Velora.Application.Common;
using Velora.Application.Services;
using Velora.Application.Services.Interface;

namespace Velora.Application
{
    public static class ApplicationDependencyContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            MapsterConfig.Register();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
