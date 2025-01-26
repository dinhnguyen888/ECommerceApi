﻿using ECommerceApi.Dtos;
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
    public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        try
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var totalProducts = await _productService.GetTotalProductsAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            if (page > totalPages && totalPages > 0)
            {
                return NotFound("Page number exceeds total pages.");
            }

            var products = await _productService.GetProductsByPageAsync(page, pageSize);

            return Ok(new
            {
                currentPage = page,
                pageSize,
                totalProducts,
                totalPages,
                products
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching products.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductPostDto product)
    {
        try
        {
            var newProductId = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = newProductId }, product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the product.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, ProductUpdateDto updatedProduct)
    {
        try
        {
            var result = await _productService.UpdateProductAsync(id, updatedProduct);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the product.", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the product.", error = ex.Message });
        }
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetSpecificationInProduct(string id)
    {
        try
        {
            var product = await _productService.GetSpecificationInProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
        }
    }

    [HttpGet("related/")]
    public async Task<IActionResult> GetRelatedProduct()
    {
        try
        {
            var product = await _productService.GetRelatedProduct();
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
        }
    }

    [HttpGet("tag/")]
    public async Task<IActionResult> GetProductByTag([FromQuery] string tag)
    {
        try
        {
            var product = await _productService.GetProductsByTagAsync(tag);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
        }
    }

    [HttpGet("search/")]
    public async Task<IActionResult> SearchProduct([FromQuery] string keyword)
    {
        try
        {
            var product = await _productService.SearchProductsAsync(keyword);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
        }
    }


}

