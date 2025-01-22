using ECommerceApi.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(TokenGenerateDto dto);
        ClaimsPrincipal ValidateToken(string token);
        
    }
}