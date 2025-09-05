using Application.Interfaces;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller.v1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/product")]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null");
            }

            await _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest("Product ID mismatch");
            }

            var existingProduct = await _productService.GetProduct(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var existingProduct = await _productService.GetProduct(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(id);
            return NoContent();
        }

    }
}
