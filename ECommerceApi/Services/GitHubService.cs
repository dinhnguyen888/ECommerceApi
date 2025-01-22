using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ECommerceApi.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IConfiguration _configuration;
        public GitHubService
            (
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IConfiguration configuration
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _configuration = configuration;
        }
        public async Task<string> GetAccessToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            var authenticateResult = await httpContext.AuthenticateAsync("GitHub");

            if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
            {
                throw new UnauthorizedAccessException("GitHub authentication failed.");
            }

            var accessToken = authenticateResult.Properties?.GetTokenValue("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("Access token is missing or invalid.");
            }

            return accessToken;
        }

        public async Task<string> GetGitHubUserData(string accessToken)
        {
            using var client = new HttpClient();
            var userAgent = _configuration["Github:UserAgent"];

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);

            var response = await client.GetAsync("https://api.github.com/user");

            if (!response.IsSuccessStatusCode)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to retrieve user data from GitHub. Error: {errorDetail}");
            }

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<(string,string)> GenerateTokenForGitHubUser(string userData, Guid userId)
        {
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(userData);
            var methodPara = new TokenGenerateDto
            {
                Email = user.login ?? user.name,
                UserName = user.name,
                RoleName = "User",
                UserId = userId,
            };
            var accessToken = _tokenService.GenerateToken(methodPara);
            var refeshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);
            return (accessToken, refeshToken.Token);
        }
    }
}
