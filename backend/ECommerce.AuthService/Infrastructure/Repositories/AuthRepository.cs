using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.AuthService.Application.Entities;
using ECommerce.AuthService.Application.Interfaces;
using ECommerce.AuthService.Infrastructure.Persistence;

namespace ECommerce.AuthService.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _db;
        public AuthRepository(AuthDbContext db)
        {
            _db = db;
        }

        public Task<bool> AnyUserExistsAsync(string userName, string email)
        {
            return _db.Users.AnyAsync(u => u.UserName == userName || u.Email == email);
        }

        public Task<User?> GetUserByUserNameOrEmailAsync(string identifier)
        {
            var lowered = identifier.ToLowerInvariant();
            return _db.Users.FirstOrDefaultAsync(u => u.UserName == identifier || u.Email == lowered);
        }

        public Task<User?> GetUserByIdAsync(string userId)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task AddUserAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }

        public Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return _db.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveRefreshTokenAsync(RefreshToken token)
        {
            _db.RefreshTokens.Remove(token);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveUserRefreshTokensAsync(string userId)
        {
            var items = await _db.RefreshTokens.Where(r => r.UserId == userId).ToListAsync();
            if (items.Count > 0)
            {
                _db.RefreshTokens.RemoveRange(items);
                await _db.SaveChangesAsync();
            }
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
