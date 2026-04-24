using Velora.Domain.ModelAggregate.Sales;
using Velora.Domain.Projection;

namespace Velora.Domain.Contracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<OrderProjection>> GetAsync();
    }
}
