using Maxishop.Web.Model;

namespace Maxishop.Web.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync();

        Task<Product> GetAsync(int id);

        Task<Product> CreateAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);

        Task<bool> IsExists(int id);
    }
}
