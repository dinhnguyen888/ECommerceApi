using ECommerceApi.Dtos;
using Microsoft.AspNetCore.Authentication;

namespace ECommerceApi.Interfaces
{
    public interface IOAuthService
    {
        Task<string> GenerateTokenFromOAuthInfo(AccountForOAuthDto dto);
        Task<string> ProcessOAuthLogin(AuthenticateResult result);
    }
}
