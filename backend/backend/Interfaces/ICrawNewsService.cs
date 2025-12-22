using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface ICrawNewsService
    {
        Task StartCrawlingAsync(int totalCrawlingPage);
        Task GetLatestDataAsync();
    }
}
