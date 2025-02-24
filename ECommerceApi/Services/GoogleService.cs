using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using PayPal.Core;

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
            // Kiểm tra email có hợp lệ không
            if (string.IsNullOrWhiteSpace(dto.email))
            {
                throw new ArgumentException("Email không hợp lệ.");
            }

            // check account exist
            var isExistingAccount = await _authService.CheckEmailExisting(dto.email);
            if (isExistingAccount == false)
            {
                // If doesn't exist, create new account
                var accountDto = new AccountPostDto
                {
                    Email = dto.email,
                    Name = dto.name,
                    Password = _configuration["DefaultPassword:User"], // Default password
                };

                bool registerResult = await _authService.RegisterAsync(accountDto);
                if (!registerResult)
                {
                    throw new Exception("Đăng ký tài khoản thất bại.");
                }
            }

            // Sign in
            string accessToken = await _authService.LoginAsync(dto.email, _configuration["DefaultPassword:User"]);
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Đăng nhập thất bại.");
            }

            return accessToken;
        }

    }
}
