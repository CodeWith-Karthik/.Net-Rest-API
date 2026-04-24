using Velora.Application.DTO.Sales.Order;
using Velora.Application.Exceptions;
using Velora.Application.Services.Interface;
using Velora.Domain.Contracts;
using Velora.Domain.Model;
using Velora.Domain.ModelAggregate.Sales;
using Velora.Domain.Projection;

namespace Velora.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthService _authService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IAuthService authService, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _authService = authService;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderProjection>> GetAsync()
        {
            return await _orderRepository.GetAsync();
        }

        public async Task CreateAsync(OrderCreateDto dto)
        {
            if (!await _authService.IsUserExists(dto.UserId))
                throw new BadRequestException("Invalid User Id");

            var productIds = dto.OrderItems.Select(x => x.ProductId).ToList();

            var products = await _productRepository.GetByIdsAsync(productIds);

            if (products.Count != productIds.Count)
                throw new BadRequestException("One or more products are invalid");

            decimal totalBillAmount = 0;

            var orderItems = new List<OrderItem>();

            var productDict = products.ToDictionary(x => x.Id);

            var groupedItems = dto.OrderItems
                                  .GroupBy(x => x.ProductId)
                                  .Select(g => new
                                  {
                                      ProductId = g.Key,
                                      Quantity = g.Sum(x => x.Quantity)
                                  });

            foreach (var item in groupedItems)
            {
                var product = productDict[item.ProductId];

                if (product.StockQuantity < item.Quantity)
                    throw new BadRequestException($"Insufficient stock for product {product.Name}");

                totalBillAmount += product.Price * item.Quantity;

                product.StockQuantity -= item.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var order = new Order
            {
                UserId = dto.UserId,
                TotalBillAmount = totalBillAmount,
                OrderItems = orderItems
            };


            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    await _productRepository.UpdateRangeAsync(products, instantSave: false);
                    await _orderRepository.CreateAsync(order, instantSave: false);
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
