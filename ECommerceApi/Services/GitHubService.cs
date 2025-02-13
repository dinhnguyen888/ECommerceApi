using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;
using System.Data;
namespace ECommerceApi.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IAccountService _accountService;
        public GitHubService
            (
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IConfiguration configuration,
            AppDbContext context,
            IAccountService accountService
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _configuration = configuration;
            _context = context;
            _accountService = accountService;
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
        public async Task<(string, string)> GenerateTokenForGitHubUser(string userData)
        {
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(userData);
            string userName = user.login;
            string userEmail = user.url;

            //get role have name call "User"
            var roleIdGet = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (roleIdGet == null) throw new ArgumentException("Role can not found");

            var roleId = int.Parse(roleIdGet.Id.ToString());

            var account = await  _accountService.GetAccountByName(userName);
            
 
            //if account not found, create new account 
            if (account == null)
            {
                var newAccount = new AccountPostDto
                {
                    Email = userEmail,
                    Name = userName,
                    Password = "Abc123@",
                  
                };
                var createdAccount = await _accountService.CreateAccountAsync(newAccount);
                 account = new AccountGetForTokenGithub
                 {
                    Id = createdAccount.Id,
                    //Email = createdAccount.Email,
                    //Name = createdAccount.Name,
                    //RoleId = roleId
                 };
            }
     
            var methodPara = new TokenGenerateDto
            {
                Email = userEmail,
                UserName = userName,
                RoleName = "User",
                UserId = account.Id,
            };

            var accessToken = _tokenService.GenerateToken(methodPara);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(account.Id);

            return (accessToken, refreshToken.Token);
        }

    }
}
