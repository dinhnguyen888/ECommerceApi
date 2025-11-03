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
using ECommerce.AuthService.Application.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IVerificationTokenService _verificationTokenService;

        public AuthService(IAuthRepository repo, IMapper mapper, IConfiguration config, IMessagePublisher messagePublisher, IVerificationTokenService verificationTokenService)
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
            _messagePublisher = messagePublisher;
            _verificationTokenService = verificationTokenService;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto dto)
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
            user.IsActive = false; // Chua verify thi khong active
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.AddUserAsync(user);

            // Tao verification token
            var verificationToken = _verificationTokenService.GenerateVerificationToken(user.Id, user.Email);
            
            // Tao verify URL
            var baseUrl = _config["AppSettings:BaseUrl"] ?? "https://localhost:7001";
            var verifyUrl = $"{baseUrl}/api/auth/verify-register?token={Uri.EscapeDataString(verificationToken)}";

            // Ban notification sau cung voi VerifyUrl de NotificationService co the gui email
            var registeredEvent = new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                RegisteredAt = user.CreatedAt,
                VerifyUrl = verifyUrl
            };
            _messagePublisher.PublishToQueue("user.registered", registeredEvent);

            return new RegisterResponseDto
            {
                VerifyUrl = verifyUrl,
                Message = "Registration successful. Please check your email for verification link."
            };
        }

        public async Task<VerifyRegistrationResponseDto> VerifyRegistrationAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new VerifyRegistrationResponseDto
                {
                    Success = false,
                    Message = "Verification token is required"
                };
            }

            var (isValid, userId, email) = _verificationTokenService.ValidateVerificationToken(token);
            
            if (!isValid || string.IsNullOrEmpty(userId))
            {
                return new VerifyRegistrationResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired verification token"
                };
            }

            var user = await _repo.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new VerifyRegistrationResponseDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            // Kiem tra email co khop khong
            if (user.Email != email)
            {
                return new VerifyRegistrationResponseDto
                {
                    Success = false,
                    Message = "Email mismatch"
                };
            }

            // Kich hoat nguoi dung
            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateUserAsync(user);

            return new VerifyRegistrationResponseDto
            {
                Success = true,
                Message = "Email verified successfully",
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

        // Cac private method de lay thoi gian token va refresh token
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
