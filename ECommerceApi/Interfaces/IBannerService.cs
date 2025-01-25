using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAllBanner();
        Task<List<Banner>> AddBaneer(BannerPostDto banner);
        Task<bool> UpdateBaneer(string id, BannerUpdateDto banner);
        Task<bool> DeleteBaneer(string id);
    }
}
