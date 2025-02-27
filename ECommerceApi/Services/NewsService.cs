using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ECommerceApi.Service
{
    public class NewsService : INewsService
    {
        private readonly IMongoCollection<News> _newsCollection;

        public NewsService(MongoDbContext dbContext)
        {
            _newsCollection = dbContext.GetCollection<News>("News");
        }

        public List<News> GetAllNews()
        {
            return _newsCollection.Find(news => true).ToList();
        }

        public List<News> GetNewsWithPagination(int pageNumber, int pageSize)
        {
            return _newsCollection
                        .Find(news => true)
                        .Skip((pageNumber - 1) * pageSize)
                        .Limit(pageSize)
                        .ToList();
        }


        public News GetNewsById(string id)
        {
            return _newsCollection.Find(news => news.Id == id).FirstOrDefault();
        }

        public void AddNews(News news)
        {
            _newsCollection.InsertOne(news);
        }

        public void UpdateNews(string id, News updatedNews)
        {
            _newsCollection.ReplaceOne(news => news.Id == id, updatedNews);
        }

        public void DeleteNews(string id)
        {
            _newsCollection.DeleteOne(news => news.Id == id);
        }
    }
}
