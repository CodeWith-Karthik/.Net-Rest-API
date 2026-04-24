using System.Linq.Expressions;
using Velora.Domain.Model;

namespace Velora.Domain.Contracts
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<List<T>> GetAsync(params Expression<Func<T, object>>[] includes);

        Task<T> GetAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<T> CreateAsync(T entity, bool instantSave = true);

        Task UpdateAsync(T entity, bool instantSave = true);

        Task DeleteAsync(T entity);

        Task<bool> IsRecordExists(int id);
    }
}
