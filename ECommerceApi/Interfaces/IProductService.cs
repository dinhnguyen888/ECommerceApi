using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface IProductService
    {
        Task<string> AddProductAsync(ProductPostDto product);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<bool> UpdateProductAsync(string id, ProductUpdateDto updatedProduct);
        Task<bool> DeleteProductAsync(string id); 
    }
}
