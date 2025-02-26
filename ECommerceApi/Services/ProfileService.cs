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


        public async Task<ProfileGetDto> GetProfileAsync(string token)
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
            return _mapper.Map<ProfileGetDto>(account);
        }

        public async Task<bool> UpdateProfileAsync(string token, ProfileUpdateDto profileUpdateDto)
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

            _mapper.Map(profileUpdateDto, account);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProfileImageAsync(string token, string pictureUrl)
        {
            // use TryParse to validate token
            if (!Guid.TryParse(_tokenService.ValidateTokenAndGetUserId(token), out Guid accountId))
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == accountId);

            // Check
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            // Update
            account.PictureUrl = pictureUrl;

            // Save
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> ChangePassword(string token, ChangePasswordDto changePasswordDto)
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

            if (!_passwordHelper.VerifyPassword(changePasswordDto.OldPassword, account.Password))
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            account.Password = _passwordHelper.HashPassword(changePasswordDto.NewPassword);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
