using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using PayPal.Core;
using System.Security.Claims;

namespace ECommerceApi.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IConfiguration _configuration;

        private readonly IAuthService _authService;

        public OAuthService(IConfiguration configuration , IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<string?> ProcessOAuthLogin(AuthenticateResult authenticateResult)
        {
            // Check if authenticateResult is null or not succeeded
            if (authenticateResult == null || !authenticateResult.Succeeded) return null;

            // Get email and name from claims
            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            // Check if email is null or empty
            if (string.IsNullOrWhiteSpace(email)) return null;

            var dto = new AccountForOAuthDto
            {
                Email = email,
                Name = name
            };

            // Generate token from OAuth info
            var accessToken = await this.GenerateTokenFromOAuthInfo(dto);
            return accessToken;

        }

        public async Task<string> GenerateTokenFromOAuthInfo(AccountForOAuthDto dto)
        {
            // Kiểm tra email có hợp lệ không
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email is invalid.");
            }

            // check account exist
            var isExistingAccount = await _authService.CheckEmailExisting(dto.Email);
            if (isExistingAccount == false)
            {
                // If doesn't exist, create new account
                var accountDto = new AccountPostDto
                {
                    Email = dto.Email,
                    Name = dto.Name,
                    Password = _configuration["DefaultPassword:User"], // Default password
                };

                bool registerResult = await _authService.RegisterAsync(accountDto);
                if (!registerResult)
                {
                    throw new Exception("Fail to register account from OAuth.");
                }
            }

            // Sign in
            string accessToken = await _authService.LoginAsync(dto.Email, _configuration["DefaultPassword:User"]);
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Fail to login account from OAuth.");
            }

            return accessToken;
        }

    }
}
