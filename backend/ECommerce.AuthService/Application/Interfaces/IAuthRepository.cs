using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerce.AuthService.Application.Entities;

namespace ECommerce.AuthService.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> AnyUserExistsAsync(string userName, string email);
        Task<User?> GetUserByUserNameOrEmailAsync(string identifier);
        Task<User?> GetUserByIdAsync(string userId);
        Task AddUserAsync(User user);

        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task AddRefreshTokenAsync(RefreshToken token);
        Task RemoveRefreshTokenAsync(RefreshToken token);
        Task RemoveUserRefreshTokensAsync(string userId);

        Task SaveChangesAsync();
    }
}
