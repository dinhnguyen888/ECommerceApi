using System.Threading.Tasks;
using ECommerce.AuthService.Application.Dtos;

namespace ECommerce.AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string userId, string refreshToken);
    }
}
