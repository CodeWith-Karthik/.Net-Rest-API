using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;
using Velora.Application.Common;
using Velora.Application.DTO.Brand;
using Velora.Application.Services.Interface;

namespace Velora.Web.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = CustomRole.WorkspaceAdmin)]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BrandDto>> Create([FromBody] BrandCreateDto dto)
        {
            var brand = await _brandService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BrandDto>>> Get()
        {
            return Ok(await _brandService.GetAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BrandDto>>> GetById(int id)
        {
            var result = await _brandService.GetAsync(id);

            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(int id, [FromBody] BrandUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            if (!await _brandService.IsRecordExistsAsync(id)) return NotFound();

            await _brandService.UpdateAsync(dto);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.JsonPatch)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<BrandUpdateDto> patchDoc)
        {
            if (patchDoc is null) return BadRequest();

            if (!await _brandService.IsRecordExistsAsync(id)) return NotFound();

            await _brandService.PatchAsync(id, patchDoc);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _brandService.IsRecordExistsAsync(id)) return NotFound();

            await _brandService.DeleteAsync(id);

            return NoContent();
        }
    }
}
