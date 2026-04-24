using Velora.Domain.Model;

namespace Velora.Domain.Contracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //Task<List<Product>> GetWithIncludeAsync();

        //Task<Product> GetByIdWithIncludeAsync(int id);

        Task<List<Product>> GetByIdsAsync(List<int> ids);

        Task UpdateRangeAsync(List<Product> products, bool instantSave = true);
    }
}
