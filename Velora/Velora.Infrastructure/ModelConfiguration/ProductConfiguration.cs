using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Model;

namespace Velora.Infrastructure.ModelConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.BrandId, x.CategoryId });

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.BrandId).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();

            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.StockQuantity).IsRequired().HasDefaultValue(0);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Product_Price_NonNegative", "\"Price\" >=0");
                t.HasCheckConstraint("CK_Product_StockQuantity_NonNegative", "\"StockQuantity\" >=0");
            });

            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.ModifiedOn).IsRequired();

            builder.HasOne(x => x.Brand).WithMany(x => x.Products).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Category).WithMany(x => x.Products).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
