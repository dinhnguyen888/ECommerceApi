using ECommerceApi.Dtos;

namespace ECommerceApi.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(AccountPostDto account);
    }
}
