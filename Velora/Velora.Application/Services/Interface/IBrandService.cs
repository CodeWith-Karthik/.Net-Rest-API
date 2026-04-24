using Microsoft.AspNetCore.JsonPatch;
using Velora.Application.DTO.Brand;

namespace Velora.Application.Services.Interface
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAsync();

        Task<BrandDto> GetAsync(int id);

        Task<BrandDto> CreateAsync(BrandCreateDto dto);

        Task UpdateAsync(BrandUpdateDto dto);

        Task PatchAsync(int id, JsonPatchDocument<BrandUpdateDto> patchDoc);

        Task DeleteAsync(int id);

        Task<bool> IsRecordExistsAsync(int id);

    }
}
