using ECommerceApi.Interfaces;
using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApi.Helpers;
namespace ECommerceApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context; 
        private readonly IMapper _mapper;
        private readonly PasswordHelper _passwordHelper;
        private readonly ITokenService _tokenService;

        public AccountService(AppDbContext context, IMapper mapper, PasswordHelper passwordHelper, ITokenService tokenService)
        {
            _context = context;
            _mapper = mapper;
            _passwordHelper = passwordHelper;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<AccountGetDto>> GetAllAccountsAsync()
        {
            var accounts = await _context.Set<Account>().Include(a => a.Role).ToListAsync();
            return _mapper.Map<IEnumerable<AccountGetDto>>(accounts);
        }

        public async Task<AccountGetDto> GetAccountByIdAsync(Guid id)
        {
            var account = await _context.Set<Account>().Include(a => a.Role).FirstOrDefaultAsync(a => a.Id == id);
            if (account == null) return null;
            return _mapper.Map<AccountGetDto>(account);
        }

        public async Task<AccountGetDto> CreateAccountAsync(AccountPostDto accountDto)
        {
            var checkRole = await _context.Roles.SingleOrDefaultAsync(r => r.Id == accountDto.RoleId);

            //check Role
            if (checkRole == null) throw new ArgumentException("Role can not found");
            var account = _mapper.Map<Account>(accountDto);
            account.Id = Guid.NewGuid();
            account.Password = _passwordHelper.HashPassword(accountDto.Password); 
            _context.Set<Account>().Add(account);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountGetDto>(account);
        }

        public async Task<AccountGetDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto)
        {
            var account = await _context.Set<Account>().FirstOrDefaultAsync(a => a.Id == id);
            if (account == null) return null;

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
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Account> CheckLegitAccount(string email)
        {
            var account = await _context.Accounts.Include(a => a.Role).SingleOrDefaultAsync(a => a.Email == email);
            if (account != null) throw new ArgumentException("account can not be found");
            return account;
        }

        public async Task<AccountGetDto> GetAccountByToken(string token)
        {
            var userId = _tokenService.ValidateTokenAndGetUserId(token);
            if (userId == null) return null;
            var profile = await _context.Accounts.FirstOrDefaultAsync(a => a.Id.ToString() == userId);
            var profileMapping = _mapper.Map<AccountGetDto>(profile);

            return profileMapping;
        }

    }
}
