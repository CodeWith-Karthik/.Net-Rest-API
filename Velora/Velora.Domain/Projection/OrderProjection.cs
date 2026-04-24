
namespace Velora.Domain.Projection
{
    public class OrderProjection
    {
        public int Id { get; set; }

        public string Customer { get; set; }

        public decimal TotalBillAmount { get; set; }

        public List<OrderItemProjection> OrderItems { get; set; }
    }

    public class OrderItemProjection
    {
        public int ProductId { get; set; }

        public string Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
