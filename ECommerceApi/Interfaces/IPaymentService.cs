using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentGetDto>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentGetDto>> GetPaymentsByAccountIdAsync(Guid accountId);
        Task<string> CreatePaymentAsync(PaymentPostDto payment);
        Task<PaymentGetDto> UpdatePaymentAsync(Guid id, PaymentUpdateDto payment);
        Task<bool> DeletePaymentAsync(Guid id);
        //Task<PaymentGetDto> ChangePaymentStatusAndGetPaymentInfo(bool paymentStatus, string description);
        //Task SendEmailUsingPaymentInfo(PaymentGetDto paymentResult);
        
    }
}
