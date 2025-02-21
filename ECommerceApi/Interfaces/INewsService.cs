using ECommerceApi.Models;
using System.Collections.Generic;

namespace ECommerceApi.Interfaces
{
    public interface INewsService
    {
        List<News> GetAllNews();
        News GetNewsById(string id);
        void AddNews(News news);
        void UpdateNews(string id, News updatedNews);
        void DeleteNews(string id);
    }
}
