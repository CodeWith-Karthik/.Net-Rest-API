using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Velora.Application.DTO.Category;
using Velora.Application.Services.Interface;
using Velora.Domain.Contracts;
using Velora.Domain.Model;

namespace Velora.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetAsync()
        {
            var result = await _categoryRepository.GetAsync();

            return result.Adapt<List<CategoryDto>>();
        }

        public async Task<CategoryDto> GetAsync(int id)
        {
            var result = await _categoryRepository.GetAsync(id);

            return result.Adapt<CategoryDto>();
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = dto.Adapt<Category>();

            var addEntity = await _categoryRepository.CreateAsync(category);

            return addEntity.Adapt<CategoryDto>();
        }

        public async Task UpdateAsync(CategoryUpdateDto dto)
        {
            var category = await _categoryRepository.GetAsync(dto.Id);

            dto.Adapt(category);

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task PatchAsync(int id, JsonPatchDocument<CategoryUpdateDto> patchDoc)
        {
            var category = await _categoryRepository.GetAsync(id);

            // Entity → DTO
            var dto = category.Adapt<CategoryUpdateDto>();

            // Apply patch
            patchDoc.ApplyTo(dto);

            // DTO → Entity
            dto.Adapt(category);

            // Save
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<bool> IsRecordExistsAsync(int id)
        {
            return await _categoryRepository.IsRecordExists(id);
        }
    }
}
