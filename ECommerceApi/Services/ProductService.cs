using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
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

    public async Task<string> AddProductAsync(ProductPostDto product)
    {
        var newProduct = _mapper.Map<Product>(product);
        await _productCollection.InsertOneAsync(newProduct);
        return newProduct.Id;
    }

    public async Task<IEnumerable<ProductGetDto>> GetProductsByPageAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        var products = await _productCollection.Find(_ => true)
                                               .Skip(skip)
                                               .Limit(pageSize)
                                               .ToListAsync();

        return _mapper.Map<IEnumerable<ProductGetDto>>(products);
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return (int)await _productCollection.CountDocumentsAsync(_ => true);
    }

    public async Task<ProductGetDto> GetProductByIdAsync(string id)
    {
        var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<ProductGetDto>(product);
    }

    public async Task<bool> UpdateProductAsync(string id, ProductUpdateDto updatedProduct)
    {
        var existingProduct = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (existingProduct == null)
        {
            return false;
        }

        var updated = _mapper.Map<Product>(updatedProduct);
        updated.Id = id; // Preserve the ID

        var result = await _productCollection.ReplaceOneAsync(p => p.Id == id, updated);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var result = await _productCollection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
    public async Task<ProductGetDetailDto> GetSpecificationInProduct(string id)
    {
        var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<ProductGetDetailDto>(product);
    }

    public async Task<List<ProductGetDto>> GetRelatedProduct()
    {

        var randomProducts = await _productCollection.Aggregate()
            .Sample(3) // take 3 ramdomly
            .ToListAsync();

        return _mapper.Map<List<ProductGetDto>>(randomProducts);
    }


}
