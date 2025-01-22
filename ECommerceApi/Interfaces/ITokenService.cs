using ECommerceApi.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(AccountGetDto accountDto);
        ClaimsPrincipal ValidateToken(string token);
        
    }
}