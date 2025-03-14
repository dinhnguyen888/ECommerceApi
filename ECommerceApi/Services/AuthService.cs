﻿using System.Threading.Tasks;
using AutoMapper;
using ECommerceApi.Interfaces;
using ECommerceApi.Dtos;
using ECommerceApi.Helpers;
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

        //login method
        public async Task<string> LoginAsync(string email, string password)
        {
            var account = await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.Email == email);

            if (account == null || !_passwordHelper.VerifyPassword(password, account.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var inputPara =  _mapper.Map<TokenGenerateDto>(account);
            var accessToken =  _tokenService.GenerateToken(inputPara);
          
            return (accessToken);
        }

        //register method
        public async Task<bool> RegisterAsync(AccountPostDto account)
        {
            string roleName = "User";
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                throw new UnauthorizedAccessException("Role name does not init in database.");
            }
            account.RoleId = role.Id;

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

            var accountMapping = _mapper.Map<TokenGenerateDto>(token.Account);
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

        //make a admin login method
        public async Task<(string, string)> AdminLoginAsync(string email, string password)
        {
            string roleName = "Admin";
            //check roleName is existed in database
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                throw new UnauthorizedAccessException("Invalid role name.");
            }
            var account = await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.Email == email);
          
            //add condition to check if account is null or password is not correct or role is not admin
            if (account == null || !_passwordHelper.VerifyPassword(password, account.Password) || account.Role.RoleName != roleName)
            {
                throw new UnauthorizedAccessException("Invalid username or password or roleName not have permission in this feature.");
            }
            var inputPara = _mapper.Map<TokenGenerateDto>(account);
            var accessToken = _tokenService.GenerateToken(inputPara);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(account.Id);
            return (accessToken, refreshToken.Token);
        }

        public async Task<bool> CheckEmailExisting(string email)
        {
            var account = await _context.Accounts.Include(a => a.Role).SingleOrDefaultAsync(a => a.Email == email);
            return account != null;
        }
    }
}
