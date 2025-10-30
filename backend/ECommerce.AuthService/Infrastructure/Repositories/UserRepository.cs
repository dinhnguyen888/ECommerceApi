using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.AuthService.Application.Entities;
using ECommerce.AuthService.Application.Interfaces;
using ECommerce.AuthService.Infrastructure.Persistence;

namespace ECommerce.AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _db;
        public UserRepository(AuthDbContext db)
        {
            _db = db;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _db.Users.AsNoTracking().ToListAsync();
        }

        public Task<User?> GetByIdAsync(string id)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
    }
}
