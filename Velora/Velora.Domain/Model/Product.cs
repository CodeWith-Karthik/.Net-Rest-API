using System.ComponentModel.DataAnnotations.Schema;

namespace Velora.Domain.Model
{
    public class Product : BaseModel
    {
        public string Name { get; set; }

        public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }
}
