using backend.Dtos;
using Microsoft.AspNetCore.Authentication;

namespace backend.Interfaces
{
    public interface IOAuthService
    {
        Task<string> GenerateTokenFromOAuthInfo(AccountForOAuthDto dto);
        Task<string> ProcessOAuthLogin(AuthenticateResult result);
    }
}
