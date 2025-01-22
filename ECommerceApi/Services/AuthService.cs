using System.Threading.Tasks;
using AutoMapper;
using Backend_e_commerce_website.Interfaces;
using ECommerceApi.Dtos;
using ECommerceApi.Helpers;
using ECommerceApi.Interfaces;
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

        public AuthService(
            ITokenService tokenService,
            PasswordHelper passwordHelper,
            ILogger<AuthService> logger,
            IAccountService accountService,
            IMapper mapper)
        {
            _tokenService = tokenService;
            _passwordHelper = passwordHelper;
            _logger = logger;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var account = await _accountService.CheckLegitAccount(email);

            if (account == null || !_passwordHelper.VerifyPassword(password, account.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var accountMapping = _mapper.Map<AccountGetDto>(account);
            return _tokenService.GenerateToken(accountMapping);
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


    }
}
