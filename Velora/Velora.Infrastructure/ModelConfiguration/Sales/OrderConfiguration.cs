using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.ModelAggregate.Sales;

namespace Velora.Infrastructure.ModelConfiguration.Sales
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.TotalBillAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.AppUser).WithMany(x => x.Orders).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
