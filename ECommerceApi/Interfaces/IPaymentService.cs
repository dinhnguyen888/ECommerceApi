using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentGetDto>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentGetDto>> GetPaymentsByAccountIdAsync(Guid accountId);
        Task<PaymentGetDto> CreatePaymentAsync(PaymentPostDto payment);
        Task<PaymentGetDto> UpdatePaymentAsync(Guid id, PaymentUpdateDto payment);
        Task<bool> DeletePaymentAsync(Guid id);
        string ConvertPaymentInfoToDescriptionInVnpay(PaymentPostDto payment);
        Task<bool> SplitDescriptionAndCreatePayment(string description);
    }
}
