using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductPostDto product)
    {
        var newProductId = await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = newProductId }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, ProductUpdateDto updatedProduct)
    {
        var result = await _productService.UpdateProductAsync(id, updatedProduct);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
