using Microsoft.EntityFrameworkCore;
using Velora.Domain.Contracts;
using Velora.Domain.ModelAggregate.Sales;
using Velora.Domain.Projection;
using Velora.Infrastructure.DbContexts;

namespace Velora.Infrastructure.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrderProjection>> GetAsync()
        {
            return await _dbContext.Order.Select(x => new OrderProjection
            {
                Id = x.Id,
                Customer = x.AppUser.Firstname + " " + x.AppUser.Lastname,
                TotalBillAmount = x.TotalBillAmount,
                OrderItems = x.OrderItems.Select(z => new OrderItemProjection
                {
                    ProductId = z.ProductId,
                    Product = z.Product.Name,
                    Price = z.Product.Price,
                    Quantity = z.Quantity
                }).ToList()
            }).ToListAsync();
        }
    }
}
