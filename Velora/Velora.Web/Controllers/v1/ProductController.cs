using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Velora.Application.Common;
using Velora.Application.DTO.Product;
using Velora.Application.Services.Interface;

namespace Velora.Web.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = CustomRole.WorkspaceAdmin)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
        {
            var product = await _productService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpGet]
        [AllowAnonymous]
        //[ResponseCache(Duration = 60)]
        [ResponseCache(CacheProfileName = "1Min")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            return Ok(await _productService.GetAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductDto>>> GetById(int id)
        {
            var result = await _productService.GetAsync(id);

            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = CustomRole.WorkspaceAdmin)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            if (!await _productService.IsRecordExistsAsync(id)) return NotFound();

            await _productService.UpdateAsync(dto);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = CustomRole.WorkspaceAdmin)]
        [Consumes(MediaTypeNames.Application.JsonPatch)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProductUpdateDto> patchDoc)
        {
            if (patchDoc is null) return BadRequest();

            if (!await _productService.IsRecordExistsAsync(id)) return NotFound();

            await _productService.PatchAsync(id, patchDoc);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = CustomRole.WorkspaceAdmin)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _productService.IsRecordExistsAsync(id)) return NotFound();

            await _productService.DeleteAsync(id);

            return NoContent();
        }
    }
}
