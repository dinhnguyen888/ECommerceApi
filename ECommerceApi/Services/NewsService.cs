using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Service
{
    public class NewsService : INewsService
    {
        private readonly IMongoCollection<News> _newsCollection;

        public NewsService(MongoDbContext dbContext)
        {
            _newsCollection = dbContext.GetCollection<News>("News");
        }

        public async Task<List<News>> GetAllNewsAsync()
        {
            return await _newsCollection.Find(news => true).ToListAsync();
        }

        public async Task<List<News>> GetNewsWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _newsCollection
                            .Find(news => true)
                            .Skip((pageNumber - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync();
        }

        public async Task<News> GetNewsByIdAsync(string id)
        {
            return await _newsCollection.Find(news => news.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddNewsAsync(News news)
        {
            await _newsCollection.InsertOneAsync(news);
        }

        public async Task UpdateNewsAsync(string id, News updatedNews)
        {
            await _newsCollection.ReplaceOneAsync(news => news.Id == id, updatedNews);
        }

        public async Task DeleteNewsAsync(string id)
        {
            await _newsCollection.DeleteOneAsync(news => news.Id == id);
        }
    }
}
