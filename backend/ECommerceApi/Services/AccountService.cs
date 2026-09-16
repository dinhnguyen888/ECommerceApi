using ECommerceApi.Interfaces;
using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApi.Helpers;
using Microsoft.IdentityModel.Tokens;
namespace ECommerceApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context; 
        private readonly IMapper _mapper;
        private readonly PasswordHelper _passwordHelper;
        private readonly ITokenService _tokenService;
        private readonly ICartService cartService;
        private readonly IConfiguration _configuration;

        public AccountService(AppDbContext context, IMapper mapper, PasswordHelper passwordHelper, ITokenService tokenService, ICartService cartService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _passwordHelper = passwordHelper;
            _tokenService = tokenService;
            this.cartService = cartService;
            _configuration = configuration;
        }

        public async Task<(List<AccountGetDto> accounts, int totalAccounts)> GetAccountsAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var totalAccounts = await _context.Set<Account>().CountAsync(); // Đếm tổng số tài khoản
            var accounts = await _context.Set<Account>()
                                         .Include(a => a.Role)
                                         .Skip(skip)
                                         .Take(pageSize)
                                         .ToListAsync();

            return (_mapper.Map<List<AccountGetDto>>(accounts), totalAccounts);
        }

        public async Task<AccountGetDto> GetAccountByIdAsync(Guid id)
        {
            var account = await _context.Set<Account>().Include(a => a.Role).FirstOrDefaultAsync(a => a.Id == id);
            if (account == null) return null;
            return _mapper.Map<AccountGetDto>(account);
        }

        public async Task<AccountGetDto> CreateAccountAsync(AccountPostDto accountDto)
        {
            Role role;

            if (accountDto.RoleId == 0) // If RoleId is not provided or invalid, set default role to "User"
            {
                role = await _context.Roles.SingleOrDefaultAsync(r => r.RoleName == "User");

                if (role == null)
                {
                    throw new Exception("Default role 'User' does not exist!");
                }
            }
            else
            {
                role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == accountDto.RoleId);

                if (role == null)
                {
                    throw new Exception("Role does not exist!");
                }
            }

            var account = _mapper.Map<Account>(accountDto);
            account.Id = Guid.NewGuid();
            account.RoleId = role.Id;

            // Get the default password from config if no password is provided in the request
            var defaultPassword = _configuration["DefaultPassword:User"];
            var rawPassword = string.IsNullOrEmpty(accountDto.Password) ? defaultPassword : accountDto.Password;
            account.Password = _passwordHelper.HashPassword(rawPassword);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Set<Account>().Add(account);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return _mapper.Map<AccountGetDto>(account);
        }





        public async Task<AccountGetDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto)
        {
            var account = await _context.Set<Account>().FirstOrDefaultAsync(a => a.Id == id);
            if (account == null) throw new Exception("account not found");


            _mapper.Map(accountDto, account);
            _context.Set<Account>().Update(account);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountGetDto>(account);
        }

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            var account = await _context.Set<Account>().FirstOrDefaultAsync(a => a.Id == id);
            if (account == null) return false;

            _context.Set<Account>().Remove(account);
            var deletedCart = cartService.ClearCart(account.Id.ToString());
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Account> CheckLegitAccount(string email)
        {
            var account = await _context.Accounts.Include(a => a.Role).SingleOrDefaultAsync(a => a.Email == email);
           if (account != null) throw new Exception("account is already exist") ;
            return account;
        }

    }
}
