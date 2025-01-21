using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task UpdateProductAsync(string id, Product updatedProduct);
        Task DeleteProductAsync(string id);
    }
}
