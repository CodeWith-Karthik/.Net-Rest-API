using System.ComponentModel.DataAnnotations;
using Velora.Application.DTO.Sales.OrderItem;

namespace Velora.Application.DTO.Sales.Order
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }

        public List<OrderItemCreateDto> OrderItems { get; set; }
    }
}
