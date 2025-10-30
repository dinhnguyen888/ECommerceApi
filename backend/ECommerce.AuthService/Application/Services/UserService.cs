using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using ECommerce.AuthService.Application.Dtos;
using ECommerce.AuthService.Application.Entities;
using ECommerce.AuthService.Application.Interfaces;

namespace ECommerce.AuthService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthRepository _authRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IAuthRepository authRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _authRepo = authRepo;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => _mapper.Map<UserDto>(u)).ToList();
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _userRepo.GetByIdAsync(id) ?? throw new ArgumentException("User not found");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(UserCreateDto dto)
        {
            var exists = await _authRepo.AnyUserExistsAsync(dto.UserName.Trim(), dto.Email.Trim().ToLowerInvariant());
            if (exists) throw new InvalidOperationException("User already exists");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.UserName.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Enum.Parse<UserRole>(dto.Role, true),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userRepo.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(string id, UserUpdateDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id) ?? throw new ArgumentException("User not found");
            user.UserName = dto.UserName.Trim();
            user.Email = dto.Email.Trim().ToLowerInvariant();
            user.Role = Enum.Parse<UserRole>(dto.Role, true);
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userRepo.GetByIdAsync(id) ?? throw new ArgumentException("User not found");
            await _userRepo.DeleteAsync(user);
            await _authRepo.RemoveUserRefreshTokensAsync(id);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var tokenUserId = GetUserIdFromAccessToken(dto.AccessToken);
            if (string.IsNullOrEmpty(tokenUserId)) throw new UnauthorizedAccessException("Invalid access token");
            if (tokenUserId != dto.UserId) throw new UnauthorizedAccessException("Forbidden");

            var user = await _userRepo.GetByIdAsync(dto.UserId) ?? throw new ArgumentException("User not found");
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password)) throw new UnauthorizedAccessException("Invalid current password");
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);
        }

        public async Task DeactivateAccountAsync(DeactivateAccountDto dto)
        {
            var tokenUserId = GetUserIdFromAccessToken(dto.AccessToken);
            if (string.IsNullOrEmpty(tokenUserId)) throw new UnauthorizedAccessException("Invalid access token");
            if (tokenUserId != dto.UserId) throw new UnauthorizedAccessException("Forbidden");

            var user = await _userRepo.GetByIdAsync(dto.UserId) ?? throw new ArgumentException("User not found");
            user.IsActive = !dto.Deactivate ? user.IsActive : false;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);
            await _authRepo.RemoveUserRefreshTokensAsync(user.Id);
        }

        private static string GetUserIdFromAccessToken(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(accessToken);
                return jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
