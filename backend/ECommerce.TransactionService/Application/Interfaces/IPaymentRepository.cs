using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho Payment Repository
    public interface IPaymentRepository
    {
        // Lay thanh toan theo ID
        Task<Payment?> GetByIdAsync(string id);

        // Lay thanh toan theo TransactionId (tu VNPay)
        Task<Payment?> GetByTransactionIdAsync(string transactionId);

        // Lay tat ca thanh toan cua don hang
        Task<List<Payment>> GetByOrderIdAsync(string orderId);

        // Tao thanh toan moi
        Task<Payment> CreateAsync(Payment payment);

        // Cap nhat thanh toan
        Task UpdateAsync(Payment payment);
    }
}
