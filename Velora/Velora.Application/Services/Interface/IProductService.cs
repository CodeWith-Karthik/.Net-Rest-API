using Microsoft.AspNetCore.JsonPatch;
using Velora.Application.DTO.Product;

namespace Velora.Application.Services.Interface
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAsync();

        Task<ProductDto> GetAsync(int id);

        Task<ProductDto> CreateAsync(ProductCreateDto dto);

        Task UpdateAsync(ProductUpdateDto dto);

        Task PatchAsync(int id, JsonPatchDocument<ProductUpdateDto> patchDoc);

        Task DeleteAsync(int id);

        Task<bool> IsRecordExistsAsync(int id);
    }
}
