using ECommerceApi.Dtos;

namespace ECommerceApi.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task<(string, string)> AdminLoginAsync(string username, string password);
        Task<bool> RegisterAsync(AccountPostDto account);
        Task<string> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task<bool> CheckEmailExisting(string email);
    }
}
