using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Infrastructure.Persistence;

namespace ECommerce.TransactionService.Infrastructure.Repositories
{
    // Repository implementation cho Payment
    public class PaymentRepository : IPaymentRepository
    {
        private readonly TransactionDbContext _db;

        public PaymentRepository(TransactionDbContext db)
        {
            _db = db;
        }

        public async Task<Payment?> GetByIdAsync(string id)
        {
            return await _db.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment?> GetByTransactionIdAsync(string transactionId)
        {
            return await _db.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<List<Payment>> GetByOrderIdAsync(string orderId)
        {
            return await _db.Payments
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.PaidAt)
                .ToListAsync();
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
            return payment;
        }

        public async Task UpdateAsync(Payment payment)
        {
            _db.Payments.Update(payment);
            await _db.SaveChangesAsync();
        }
    }
}
