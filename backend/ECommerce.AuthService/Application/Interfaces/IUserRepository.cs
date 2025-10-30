using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.AuthService.Application.Entities;

namespace ECommerce.AuthService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
