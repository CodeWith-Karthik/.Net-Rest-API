using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Velora.Application.Common;
using Velora.Application.DTO.Category;
using Velora.Application.Services.Interface;

namespace Velora.Web.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryCreateDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> Get()
        {
            _logger.LogInformation("Fetching data for categoires");

            return Ok(await _categoryService.GetAsync());
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CategoryDto>>> GetById(int id)
        {

            _logger.LogInformation($"Fetching data for category id:{id}");

            var result = await _categoryService.GetAsync(id);

            if (result is null)
            {
                _logger.LogWarning($"Category for id:{id} not found");
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            if (!await _categoryService.IsRecordExistsAsync(id)) return NotFound();

            await _categoryService.UpdateAsync(dto);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.JsonPatch)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<CategoryUpdateDto> patchDoc)
        {
            if (patchDoc is null) return BadRequest();

            if (!await _categoryService.IsRecordExistsAsync(id)) return NotFound();

            await _categoryService.PatchAsync(id, patchDoc);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _categoryService.IsRecordExistsAsync(id)) return NotFound();

            await _categoryService.DeleteAsync(id);

            return NoContent();
        }
    }
}
