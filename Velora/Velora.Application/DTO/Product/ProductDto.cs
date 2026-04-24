namespace Velora.Application.DTO.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BrandId { get; set; }

        public string Brand { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }
}
