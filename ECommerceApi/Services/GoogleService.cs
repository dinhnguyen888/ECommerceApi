using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;

namespace ECommerceApi.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly IConfiguration _configuration;

        private readonly IAuthService _authService;

        public GoogleService(IConfiguration configuration , IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<string> GenerateTokenFromGoogleInfo(GoogleRequestDto dto)
        {
            var accountDto = new AccountPostDto
            {
                Email = dto.Email,
                Name = dto.Name,
                Password = _configuration["DefaultPassword:User"],
            };
            bool reusult = await _authService.RegisterAsync(accountDto); // Register the account if it doesn't exist
   
            var accessToken = await _authService.LoginAsync(accountDto.Email, accountDto.Password);
            return accessToken;
        }
    }
}
