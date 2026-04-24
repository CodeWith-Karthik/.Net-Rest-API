using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.ModelAggregate.Sales;

namespace Velora.Infrastructure.ModelConfiguration.Sales
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.HasIndex(x => new { x.OrderId, x.ProductId });

            builder.Property(x => x.ProductId).IsRequired();

            builder.HasOne(x => x.Order).WithMany(x => x.OrderItems).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
