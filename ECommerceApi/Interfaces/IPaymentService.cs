using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<(List<PaymentGetDto> payments, int totalPayments)> GetPaymentsAsync(int page, int pageSize);
        Task<IEnumerable<PaymentViewHistoryDto>> ViewPaymentHistory(string token);
        Task<string> CreatePaymentAsync(PaymentPostDto payment);
        Task<PaymentGetDto> UpdatePaymentAsync(Guid id, PaymentUpdateDto payment);
        Task<bool> DeletePaymentAsync(int id);

        Task<bool> DeletePendingPaymentAsync();
        Task<PaymentGetDto> ChangePaymentStatusAndGetPaymentInfo(bool paymentStatus, long description);
        Task SendEmailUsingPaymentInfo(PaymentGetDto paymentResult);


    }
}
