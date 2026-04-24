namespace Velora.Application.DTO.Product
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }
}
