namespace Velora.Domain.Model
{
    public class Category : BaseModel
    {
        public string Name { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
