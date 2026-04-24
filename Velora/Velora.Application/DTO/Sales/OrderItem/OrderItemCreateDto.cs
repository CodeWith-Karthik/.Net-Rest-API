using System.ComponentModel.DataAnnotations;

namespace Velora.Application.DTO.Sales.OrderItem
{
    public class OrderItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}