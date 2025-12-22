using backend.Dtos;
using backend.Models;

namespace backend.Interfaces
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
