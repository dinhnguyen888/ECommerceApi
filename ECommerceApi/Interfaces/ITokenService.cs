using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface ITokenService
    {

        string GenerateToken(Claim[] claims);
        ClaimsPrincipal ValidateToken(string token);
        Task<DateTime?> GetTokenExpirationAsync(string token);
    }
}