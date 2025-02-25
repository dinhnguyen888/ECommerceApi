using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Helpers;
using ECommerceApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Helpers;
namespace ECommerceApi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly PasswordHelper _passwordHelper = new PasswordHelper();
        public ProfileService(AppDbContext context,
            IMapper mapper,
            IEmailService emailService,
            IAccountService accountService,
            ITokenService tokenService
            )
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _accountService = accountService;
            _tokenService = tokenService;
        }


        public async Task<AccountGetDto> GetProfileAsync(string token)
        {
            Guid accountId = Guid.Parse(_tokenService.ValidateTokenAndGetUserId(token));
            if (accountId == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }
            return _mapper.Map<AccountGetDto>(account);
        }

        public async Task<AccountGetDto> UpdateProfileAsync(string token, AccountUpdateDto accountDto)
        {
            Guid accountId = Guid.Parse(_tokenService.ValidateTokenAndGetUserId(token));
            if (accountId == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }
            await _accountService.UpdateAccountAsync(accountId, accountDto);
            return _mapper.Map<AccountGetDto>(account);
        }

        public async Task<bool> ChangePassword(string token, string oldPassword, string newPassword)
        {
            Guid accountId = Guid.Parse(_tokenService.ValidateTokenAndGetUserId(token));
            if (accountId == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            if (!_passwordHelper.VerifyPassword(oldPassword, account.Password))
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            account.Password = _passwordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
