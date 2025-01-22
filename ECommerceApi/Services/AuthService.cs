using System.Threading.Tasks;
using AutoMapper;
using Backend_e_commerce_website.Interfaces;
using ECommerceApi.Dtos;
using ECommerceApi.Helpers;
using ECommerceApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly PasswordHelper _passwordHelper;
        private readonly ILogger<AuthService> _logger;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AppDbContext _context;
        

        public AuthService(
            ITokenService tokenService,
            PasswordHelper passwordHelper,
            ILogger<AuthService> logger,
            IAccountService accountService,
            IMapper mapper,
            IRefreshTokenService refreshTokenService,
            AppDbContext context)
        {
            _tokenService = tokenService;
            _passwordHelper = passwordHelper;
            _logger = logger;
            _accountService = accountService;
            _mapper = mapper;
            _refreshTokenService = refreshTokenService;
            _context = context;
        }

        public async Task<(string, string)> LoginAsync(string email, string password)
        {
            var account = await _accountService.CheckLegitAccount(email);

            if (account == null || !_passwordHelper.VerifyPassword(password, account.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var accountMapping = _mapper.Map<AccountGetDto>(account);
            var accessToken = _tokenService.GenerateToken(accountMapping);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(account.Id);
            return (accessToken, refreshToken.Token);
        }

        public async Task<bool> RegisterAsync(AccountPostDto account)
        {
            var existingAccount = await _accountService.CheckLegitAccount(account.Email);
            if (existingAccount != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            await _accountService.CreateAccountAsync(account);
            return true;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            if (!await _refreshTokenService.ValidateRefreshTokenAsync(refreshToken))
            {
                throw new Exception("Invalid or expired refresh token.");
            }

            var token = await _context.RefreshTokens
                .Include(rt => rt.Account)
                .ThenInclude(a => a.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            var accountMapping = _mapper.Map<AccountGetDto>(token.Account);
            if (token == null) throw new Exception("Invalid refresh token.");
               
            return _tokenService.GenerateToken(accountMapping);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Refresh token is required.");
            }      
                var deleteToken = await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken);
            if (!deleteToken) throw new Exception("Token can not delete!!");
            
                        
        }

    }
}
