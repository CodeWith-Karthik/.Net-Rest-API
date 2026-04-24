using Microsoft.AspNetCore.JsonPatch;
using Velora.Application.DTO.Category;

namespace Velora.Application.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAsync();

        Task<CategoryDto> GetAsync(int id);

        Task<CategoryDto> CreateAsync(CategoryCreateDto dto);

        Task UpdateAsync(CategoryUpdateDto dto);

        Task PatchAsync(int id, JsonPatchDocument<CategoryUpdateDto> patchDoc);

        Task DeleteAsync(int id);

        Task<bool> IsRecordExistsAsync(int id);
    }
}
