using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BCrypt.Net;
using ECommerce.AuthService.Application.Dtos;
using ECommerce.AuthService.Application.Entities;
using ECommerce.AuthService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IMapper mapper, IConfiguration config)
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var normalizedUserName = dto.UserName?.Trim();
            var normalizedEmail = dto.Email?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(normalizedUserName) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(normalizedEmail))
            {
                throw new ArgumentException("Invalid register data");
            }

            var exists = await _repo.AnyUserExistsAsync(normalizedUserName, normalizedEmail);
            if (exists)
            {
                throw new InvalidOperationException("User already exists");
            }

            var user = _mapper.Map<User>(dto);
            user.Id = Guid.NewGuid().ToString();
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Email = normalizedEmail;
            user.UserName = normalizedUserName;
            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.AddUserAsync(user);

            var tokens = await IssueTokensAsync(user, replaceExistingForUser: false);
            return new AuthResponseDto
            {
                AccessToken = tokens.accessToken,
                RefreshToken = tokens.refreshToken.Token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var identifier = dto.UserNameOrEmail?.Trim();
            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Invalid login data");
            }

            var user = await _repo.GetUserByUserNameOrEmailAsync(identifier);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var tokens = await IssueTokensAsync(user, replaceExistingForUser: false);
            return new AuthResponseDto
            {
                AccessToken = tokens.accessToken,
                RefreshToken = tokens.refreshToken.Token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException("Invalid refresh token");
            }

            var tokenText = refreshToken.Trim();
            var rt = await _repo.GetRefreshTokenAsync(tokenText);
            if (rt == null || rt.ExpiredAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token invalid or expired");
            }

            var user = rt.User;
            var accessToken = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = string.Empty,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task LogoutAsync(string userId, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(userId)) return;

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var tokenText = refreshToken.Trim();
                var rt = await _repo.GetRefreshTokenAsync(tokenText);
                if (rt != null)
                {
                    await _repo.RemoveRefreshTokenAsync(rt);
                    return;
                }
            }
            else
            {
                await _repo.RemoveUserRefreshTokensAsync(userId);
            }
        }

        private async Task<(string accessToken, RefreshToken refreshToken)> IssueTokensAsync(User user, bool replaceExistingForUser)
        {
            if (replaceExistingForUser)
            {
                await _repo.RemoveUserRefreshTokensAsync(user.Id);
            }

            var accessToken = GenerateJwtToken(user);
            var newRefresh = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + "." + Guid.NewGuid().ToString("N"),
                ExpiredAt = DateTime.UtcNow.AddDays(GetRefreshTokenLifetimeDays()),
                User = user
            };

            await _repo.AddRefreshTokenAsync(newRefresh);
            return (accessToken, newRefresh);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(GetAccessTokenLifetimeMinutes()),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int GetAccessTokenLifetimeMinutes()
        {
            var jwtSection = _config.GetSection("Jwt");
            return int.TryParse(jwtSection["AccessTokenLifetimeMinutes"], out var m) ? m : 30;
        }

        private int GetRefreshTokenLifetimeDays()
        {
            var jwtSection = _config.GetSection("Jwt");
            return int.TryParse(jwtSection["RefreshTokenLifetimeDays"], out var d) ? d : 14;
        }
    }
}
