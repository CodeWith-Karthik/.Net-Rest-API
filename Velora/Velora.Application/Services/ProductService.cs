using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Velora.Application.DTO.Product;
using Velora.Application.Exceptions;
using Velora.Application.Services.Interface;
using Velora.Domain.Contracts;
using Velora.Domain.Model;

namespace Velora.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ProductDto>> GetAsync()
        {
            //var result = await _productRepository.GetAsync();

            //var result = await _productRepository.GetWithIncludeAsync();

            var result = await _productRepository.GetAsync([x => x.Brand, x => x.Category]);

            return result.Adapt<List<ProductDto>>();
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            //var result = await _productRepository.GetAsync(id);

            //var result = await _productRepository.GetByIdWithIncludeAsync(id);

            var result = await _productRepository.GetAsync(id, [x => x.Brand, x => x.Category]);

            return result.Adapt<ProductDto>();
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            await ValidateProductAsync(dto);

            var product = dto.Adapt<Product>();

            var addEntity = await _productRepository.CreateAsync(product);

            return addEntity.Adapt<ProductDto>();
        }

        public async Task UpdateAsync(ProductUpdateDto dto)
        {
            await ValidateProductAsync(dto);

            var product = await _productRepository.GetAsync(dto.Id);

            dto.Adapt(product);

            await _productRepository.UpdateAsync(product);
        }

        public async Task PatchAsync(int id, JsonPatchDocument<ProductUpdateDto> patchDoc)
        {
            var product = await _productRepository.GetAsync(id);

            // Entity → DTO
            var dto = product.Adapt<ProductUpdateDto>();

            // Apply patch
            patchDoc.ApplyTo(dto);

            // DTO → Entity
            dto.Adapt(product);

            await ValidateProductAsync(dto);

            // Save
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            await _productRepository.DeleteAsync(product);
        }

        public async Task<bool> IsRecordExistsAsync(int id)
        {
            return await _productRepository.IsRecordExists(id);
        }

        private async Task ValidateProductAsync<T>(T model)
        {
            switch (model)
            {
                case ProductCreateDto create:
                    await ValidateCommonAsync(create.BrandId, create.CategoryId, create.Price, create.StockQuantity);
                    break;

                case ProductUpdateDto update:
                    await ValidateCommonAsync(update.BrandId, update.CategoryId, update.Price, update.StockQuantity);
                    break;

                case Product product:
                    await ValidateCommonAsync(product.BrandId, product.CategoryId, product.Price, product.StockQuantity);
                    break;
            }
        }

        private async Task ValidateCommonAsync(int brandId, int categoryId, decimal price, int stockQuantity)
        {
            if (!await _brandRepository.IsRecordExists(brandId)) throw new BadRequestException($"Invalid Brand Id: {brandId}");

            if (!await _categoryRepository.IsRecordExists(categoryId)) throw new BadRequestException($"Invalid Category Id: {categoryId}");

            if (price < 0) throw new BadRequestException($"Invalid Price:{price}. Price must me zero or greater");

            if (stockQuantity < 0) throw new BadRequestException($"Invalid Stock Quantity:{stockQuantity}. Stock Quantity must me zero or greater");
        }
    }
}
