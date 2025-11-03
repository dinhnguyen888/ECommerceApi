using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.AuthService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.AuthService.Application.Services
{
    public class VerificationTokenService : IVerificationTokenService
    {
        private readonly IConfiguration _config;

        public VerificationTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateVerificationToken(string userId, string email)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("type", "verification"), // Danh dau day la verification token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5), // 5 phut
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (bool isValid, string? userId, string? email) ValidateVerificationToken(string token)
        {
            try
            {
                var jwtSection = _config.GetSection("Jwt");
                var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                
                // Kiem tra xem co phai la verification token khong
                var tokenType = principal.FindFirst("type")?.Value;
                if (tokenType != "verification")
                {
                    return (false, null, null);
                }

                var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

                return (true, userId, email);
            }
            catch
            {
                return (false, null, null);
            }
        }
    }
}

