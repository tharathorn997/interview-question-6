using Microsoft.AspNetCore.Mvc;
using ExampleCom.ProductManagement.Api.Models.Dto;
using ExampleCom.ProductManagement.Api.Services;

namespace ExampleCom.ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var response = products.Select(p => new ProductResponse
        {
            Id = p.Id,
            ProductCode = p.ProductCode,
            CreatedAt = p.CreatedAt
        });
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _productService.ExistsAsync(request.ProductCode))
            return Conflict(new { message = "Product code already exists" });

        var product = await _productService.CreateAsync(request.ProductCode);
        var response = new ProductResponse
        {
            Id = product.Id,
            ProductCode = product.ProductCode,
            CreatedAt = product.CreatedAt
        };

        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
