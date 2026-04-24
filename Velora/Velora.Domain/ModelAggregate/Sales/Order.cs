using System.ComponentModel.DataAnnotations.Schema;
using Velora.Domain.Model;
using Velora.Domain.ModelAggregate.User;

namespace Velora.Domain.ModelAggregate.Sales
{
    public class Order : BaseModel
    {
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }

        public decimal TotalBillAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
