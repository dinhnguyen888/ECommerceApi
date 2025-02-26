using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    // Generate Token
    public string GenerateToken(TokenGenerateDto dto)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("email", dto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, dto.UserId.ToString()),
            new Claim("roleName", dto.RoleName ?? throw new Exception("rolename null")),
            new Claim("userId" , dto.UserId.ToString()),
            new Claim("userName", dto.UserName)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(int.Parse(jwtSettings["ExpiresInDays"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Validate Token and Get UserId
    public string ValidateTokenAndGetUserId(string token)
    {
        var principal = ValidateToken(token);

        // if principal is null, throw exception
        if (principal == null)
        {
            throw new SecurityTokenException("Token is invalid");
        }

        // get userId claim
        var userIdClaim = principal.Claims.FirstOrDefault(claim => claim.Type == "userId");
        if (userIdClaim == null)
        {
            throw new SecurityTokenException("userId can not be found in token");
        }

        return userIdClaim.Value;
    }

    // Validate Token
    private ClaimsPrincipal ValidateToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = key,
            NameClaimType = ClaimTypes.NameIdentifier
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }


    // Validate Token and Get User Id
    //public string ValidateTokenAndGetUserId(string token)
    //{
    //    var jwtToken = _tokenHandler.ReadJwtToken(token);

    //    //if jwt token is expired or null, thow exception
    //    if (jwtToken.ValidTo < DateTime.UtcNow)
    //    {
    //        throw new SecurityTokenException("Token is expired");
    //    }

    //    if (jwtToken == null)
    //    {
    //        throw new SecurityTokenException("Token is invalid");
    //    }

    //    var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");
    //    if (userIdClaim == null)
    //    {
    //        throw new SecurityTokenException("userId can not found in token");
    //    }
    //    return userIdClaim.Value;
    //}


}
