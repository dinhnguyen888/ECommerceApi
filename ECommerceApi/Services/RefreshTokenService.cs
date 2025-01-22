using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _dbContext;

    public RefreshTokenService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid accountId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            AccountId = accountId,
            ExpiryDate = DateTime.UtcNow.AddDays(7), // expired in 7 days
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);

        return refreshToken != null && refreshToken.ExpiryDate > DateTime.UtcNow;
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null) throw new Exception("Invalid refresh token.");

        refreshToken.IsRevoked = true;
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> DeleteRefreshTokenAsync(string token)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null)
        {
            throw new Exception("Refresh token not found.");
        }

        _dbContext.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();
        return refreshToken != null;
    }
}
