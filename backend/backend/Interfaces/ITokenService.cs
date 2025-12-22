using backend.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(TokenGenerateDto dto);
        string? ValidateTokenAndGetUserId(string token);


    }
}