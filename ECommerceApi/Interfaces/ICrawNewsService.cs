namespace ECommerceApi.Interfaces
{
    public interface ICrawNewsService
    {
        void StartCrawling(int totalCrawlingPage);
        void GetLatestData();
    }
}