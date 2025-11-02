using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Dtos;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho Payment Service - business logic cua Payment
    public interface IPaymentService
    {
        // Tao thanh toan va tra ve URL thanh toan VNPay
        Task<VnpayPaymentUrlDto> CreatePaymentAsync(PaymentCreateDto dto);

        // Xu ly callback tu VNPay sau khi thanh toan
        Task<bool> HandleVnpayCallbackAsync(VnpayCallbackDto callback);

        // Lay thong tin thanh toan theo ID
        Task<PaymentGetDto?> GetPaymentByIdAsync(string id);

        // Lay thong tin thanh toan theo TransactionId
        Task<PaymentGetDto?> GetPaymentByTransactionIdAsync(string transactionId);

        // Lay tat ca thanh toan (admin)
        Task<List<PaymentGetDto>> GetAllPaymentsAsync();

        // Lay tat ca thanh toan theo OrderId
        Task<List<PaymentGetDto>> GetPaymentsByOrderIdAsync(string orderId);

        // Cap nhat thong tin thanh toan
        Task<PaymentGetDto> UpdatePaymentAsync(PaymentUpdateDto dto);

        // Xoa thanh toan
        Task DeletePaymentAsync(string id);
    }
}

