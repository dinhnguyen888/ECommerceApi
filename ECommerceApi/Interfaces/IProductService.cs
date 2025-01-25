using ECommerceApi.Dtos;

public interface IProductService
{
    Task<IEnumerable<ProductGetDto>> GetProductsByPageAsync(int page, int pageSize);
    Task<int> GetTotalProductsAsync();
    Task<ProductGetDto> GetProductByIdAsync(string id);
    Task<string> AddProductAsync(ProductPostDto product);
    Task<bool> UpdateProductAsync(string id, ProductUpdateDto updatedProduct);
    Task<bool> DeleteProductAsync(string id);
    Task<ProductGetDetailDto> GetSpecificationInProduct(string id);
    Task<List<ProductGetDto>> GetRelatedProduct();
}
