using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile?> GetMyProfileAsync(string token);
        Task<UserProfile> PostMyProfileAsync(UserProfile userProfile);
        Task<UserProfile?> UpdateMyProfileAsync(string userId, UserProfile updatedProfile);
        Task<bool> DeleteMyProfileAsync(string userId);
    }
}
