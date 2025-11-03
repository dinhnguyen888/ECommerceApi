using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Infrastructure.Persistence;

namespace ECommerce.TransactionService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TransactionDbContext _db;

        public OrderRepository(TransactionDbContext db)
        {
            _db = db;
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _db.Orders
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByIdWithDetailsAsync(string id)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            return await _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _db.Orders
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetExpiredPendingOrdersAsync()
        {
            var now = DateTime.UtcNow;
            return await _db.Orders
                .Where(o => o.Status == OrderStatus.Pending && o.ExpiresAt <= now)
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var order = await GetByIdWithDetailsAsync(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }
    }
}
