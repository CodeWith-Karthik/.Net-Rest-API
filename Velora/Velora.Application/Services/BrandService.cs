using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Velora.Application.DTO.Brand;
using Velora.Application.Services.Interface;
using Velora.Domain.Contracts;
using Velora.Domain.Model;

namespace Velora.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<BrandDto>> GetAsync()
        {
            var result = await _brandRepository.GetAsync();

            return result.Adapt<List<BrandDto>>();
        }

        public async Task<BrandDto> GetAsync(int id)
        {
            var result = await _brandRepository.GetAsync(id);

            return result.Adapt<BrandDto>();
        }

        public async Task<BrandDto> CreateAsync(BrandCreateDto dto)
        {
            var brand = dto.Adapt<Brand>();

            var addEntity = await _brandRepository.CreateAsync(brand);

            return addEntity.Adapt<BrandDto>();
        }

        public async Task UpdateAsync(BrandUpdateDto dto)
        {
            var brand = await _brandRepository.GetAsync(dto.Id);

            dto.Adapt(brand);

            await _brandRepository.UpdateAsync(brand);
        }

        public async Task PatchAsync(int id, JsonPatchDocument<BrandUpdateDto> patchDoc)
        {
            var brand = await _brandRepository.GetAsync(id);

            // Entity → DTO
            var dto = brand.Adapt<BrandUpdateDto>();

            // Apply patch
            patchDoc.ApplyTo(dto);

            // DTO → Entity
            dto.Adapt(brand);

            // Save
            await _brandRepository.UpdateAsync(brand);
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(id);

            await _brandRepository.DeleteAsync(brand);
        }

        public async Task<bool> IsRecordExistsAsync(int id)
        {
            return await _brandRepository.IsRecordExists(id);
        }
    }
}
