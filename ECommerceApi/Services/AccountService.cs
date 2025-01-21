using Backend_e_commerce_website.Interfaces;
using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_e_commerce_website.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context; 
        private readonly IMapper _mapper;

        public AccountService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var account = _mapper.Map<Account>(accountDto);
            account.Id = Guid.NewGuid();
            account.Password = HashPassword(accountDto.Password); 
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

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
