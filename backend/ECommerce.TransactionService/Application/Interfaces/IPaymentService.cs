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
    }
}
