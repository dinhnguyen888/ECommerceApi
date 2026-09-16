using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
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

    public async Task<List<ProductGetDto>> GetMultipleProductByTag(List<ProductTagRequestDto> tagRequests)
    {
        var result = new List<ProductGetDto>();

        foreach (var tagRequest in tagRequests)
        {
            var products = await _productCollection.Find(p => p.Tag == tagRequest.Tag)
                                                   .Limit(tagRequest.Limit)
                                                   .ToListAsync();

            result.AddRange(_mapper.Map<List<ProductGetDto>>(products));
        }

        return result;
    }


    public async Task<(List<ProductGetDto> products, long totalProducts)> GetProductsAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

       
        var countTask = _productCollection.CountDocumentsAsync(_ => true);
        var productsTask = _productCollection.Find(_ => true)
                                             .Skip(skip)
                                             .Limit(pageSize)
                                             .ToListAsync();

        await Task.WhenAll(countTask, productsTask); 

        return (_mapper.Map<List<ProductGetDto>>(productsTask.Result), countTask.Result);
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
    public async Task<ProductGetDetailDto> GetProductDetail(string id)
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
    public async Task<(List<ProductGetDto> products, long totalProducts)> GetProductsByTagAsync(string tag, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        // Đếm tổng số sản phẩm có tag
        var totalProducts = await _productCollection.CountDocumentsAsync(p => p.Tag == tag);

        // Lấy danh sách sản phẩm theo trang
        var products = await _productCollection.Find(p => p.Tag == tag)
                                               .Skip(skip)
                                               .Limit(pageSize)
                                               .ToListAsync();

        return (_mapper.Map<List<ProductGetDto>>(products), totalProducts);
    }


    public async Task<List<ProductGetDto>> SearchProductsAsync(string keyword, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        var filter = Builders<Product>.Filter.Or(
            Builders<Product>.Filter.Regex(p => p.Title, new BsonRegularExpression(keyword, "i")), //regardless of case
            Builders<Product>.Filter.Regex(p => p.Description, new BsonRegularExpression(keyword, "i")),
            Builders<Product>.Filter.Regex(p => p.Tag, new BsonRegularExpression(keyword, "i"))
        );

        var products = await _productCollection.Find(filter)
                                               .Skip(skip)
                                               .Limit(pageSize)
                                               .ToListAsync();
        return _mapper.Map<List<ProductGetDto>>(products);
    }

    public async Task<string> GetProductUrlByIdAsync(string productId)
    {
        var product = await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();
        return product?.ProductUrl ?? string.Empty; 
    }


    public async Task<Product> GetProductForUpdating(string id)
    {
        var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product;
    }
}
