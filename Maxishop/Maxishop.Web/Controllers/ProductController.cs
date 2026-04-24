using Maxishop.Web.Data;
using Maxishop.Web.Model;
using Maxishop.Web.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace Maxishop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await _productRepository.CreateAsync(product);

            //return Ok();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var result = await _productRepository.GetAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var result = await _productRepository.GetAsync(id);

            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
        public async Task<ActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();

            if (!await _productRepository.IsExists(id)) return NotFound();

            await _productRepository.UpdateAsync(product);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.JsonPatch)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc is null) return BadRequest();

            var product = await _productRepository.GetAsync(id);

            if (product is null) return NotFound();

            patchDoc.ApplyTo(product);

            await _productRepository.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product is null) return NotFound();

            await _productRepository.DeleteAsync(product);

            return NoContent();
        }
    }
}
