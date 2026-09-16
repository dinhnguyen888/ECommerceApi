using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly IMapper _mapper;

        public CategoryService(MongoDbContext dbContext, IMapper mapper)
        {
            _categories = dbContext.GetCollection<Category>("Category");
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _categories.Find(category => category.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> CreateCategoryAsync(CategoryPostDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = ObjectId.GenerateNewId().ToString(); // Tạo ObjectId mới cho MongoDB
            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(string id, CategoryUpdateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = id; // Đảm bảo sử dụng ID hiện tại

            var result = await _categories.ReplaceOneAsync(c => c.Id == id, category);

            if (result.MatchedCount == 0) return null;
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var result = await _categories.DeleteOneAsync(category => category.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
