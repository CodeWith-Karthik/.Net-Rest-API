using Velora.Application.DTO.Sales.Order;
using Velora.Domain.Projection;

namespace Velora.Application.Services.Interface
{
    public interface IOrderService
    {
        Task<List<OrderProjection>> GetAsync();

        Task CreateAsync(OrderCreateDto dto);
    }
}
