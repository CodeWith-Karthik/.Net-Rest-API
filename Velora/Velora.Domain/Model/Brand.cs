namespace Velora.Domain.Model
{
    public class Brand : BaseModel
    {
        public string Name { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
