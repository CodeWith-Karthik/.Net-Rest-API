using Mapster;
using Velora.Application.DTO.Product;
using Velora.Domain.Model;

namespace Velora.Application.Common
{
    public class MapsterConfig
    {
        public static void Register()
        {
            TypeAdapterConfig<Product, ProductDto>
                .NewConfig()
                .Map(dest => dest.Brand, src => src.Brand.Name)
                .Map(dest => dest.Category, src => src.Category.Name);
        }
    }
}
