using ECommerceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface INewsService
    {
        Task<List<News>> GetAllNewsAsync();
        Task<List<News>> GetNewsWithPaginationAsync(int pageNumber, int pageSize);
        Task<News> GetNewsByIdAsync(string id);
        Task AddNewsAsync(News news);
        Task UpdateNewsAsync(string id, News updatedNews);
        Task DeleteNewsAsync(string id);
    }
}
