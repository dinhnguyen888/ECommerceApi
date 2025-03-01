using ECommerceApi.Dtos;
using ECommerceApi.Models;

public interface IProductService
{
    Task<(List<ProductGetDto> products, long totalProducts)> GetProductsAsync(int page, int pageSize);
    Task<ProductGetDto> GetProductByIdAsync(string id);
    Task<string> AddProductAsync(ProductPostDto product);
    Task<bool> UpdateProductAsync(string id, ProductUpdateDto updatedProduct);
    Task<bool> DeleteProductAsync(string id);
    Task<ProductGetDetailDto> GetProductDetail(string id);
    Task<(List<ProductGetDto> products, long totalProducts)> GetProductsByTagAsync(string tag, int page, int pageSize);
    Task<List<ProductGetDto>> SearchProductsAsync(string keyword, int page, int pageSize);
    Task<string> GetProductUrlByIdAsync(string productId);
    
    Task<Product> GetProductForUpdating(string id);
}
