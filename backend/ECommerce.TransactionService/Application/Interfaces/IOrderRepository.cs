using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho Order Repository
    public interface IOrderRepository
    {
        // Lay don hang theo ID
        Task<Order?> GetByIdAsync(string id);

        // Lay don hang theo ID kem theo OrderItems va Payments
        Task<Order?> GetByIdWithDetailsAsync(string id);

        // Lay tat ca don hang cua user
        Task<List<Order>> GetByUserIdAsync(string userId);

        // Lay tat ca don hang
        Task<List<Order>> GetAllAsync();

        // Lay tat ca don hang pending da het han (expired)
        Task<List<Order>> GetExpiredPendingOrdersAsync();

        // Tao don hang moi
        Task<Order> CreateAsync(Order order);

        // Cap nhat don hang
        Task UpdateAsync(Order order);

        // Xoa don hang (soft delete hoac hard delete tuy design)
        Task DeleteAsync(string id);
    }
}
