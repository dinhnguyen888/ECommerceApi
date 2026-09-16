using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface ICrawNewsService
    {
        Task StartCrawlingAsync(int totalCrawlingPage);
        Task GetLatestDataAsync();
    }
}
