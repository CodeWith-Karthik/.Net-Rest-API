using System.ComponentModel.DataAnnotations.Schema;
using Velora.Domain.Model;

namespace Velora.Domain.ModelAggregate.Sales
{
    public class OrderItem
    {
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
