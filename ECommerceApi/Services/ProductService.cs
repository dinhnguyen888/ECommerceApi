using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Driver;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductService(MongoDbContext dbContext)
    {
        _productCollection = dbContext.GetCollection<Product>("Product");
    }

    public async Task AddProductAsync(Product product)
    {
        await _productCollection.InsertOneAsync(product);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateProductAsync(string id, Product updatedProduct)
    {
        await _productCollection.ReplaceOneAsync(p => p.Id == id, updatedProduct);
    }

    public async Task DeleteProductAsync(string id)
    {
        await _productCollection.DeleteOneAsync(p => p.Id == id);
    }
}
