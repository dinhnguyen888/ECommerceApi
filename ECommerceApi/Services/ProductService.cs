using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Driver;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _productCollection;
    private readonly IMapper _mapper;

    public ProductService(MongoDbContext dbContext, IMapper mapper)
    {
        _productCollection = dbContext.GetCollection<Product>("Product");
        _mapper = mapper;
    }

    public async Task<string> AddProductAsync(ProductPostDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _productCollection.InsertOneAsync(product);
        return product.Id;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateProductAsync(string id, ProductUpdateDto dto)
    {
        var productExists = await _productCollection.Find(p => p.Id == id).AnyAsync();

        if (!productExists)
        {
            return false;
        }

        var product = _mapper.Map<Product>(dto);
        product.Id = id; // Preserve the ID
        await _productCollection.ReplaceOneAsync(p => p.Id == id, product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var deleteResult = await _productCollection.DeleteOneAsync(p => p.Id == id);
        return deleteResult.DeletedCount > 0;
    }
}
