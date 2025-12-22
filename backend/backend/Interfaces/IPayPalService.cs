using backend.Dtos;

namespace backend.Interfaces
{
    public interface IPayPalService
    {
        Task<string> CreatePaymentInPayPal(PaymentPostDto dto);
        Task<string> CreatePaymentUrl(string paymentId, double price, string productId, string productName);
        Task PaypalCallbackHandle(IQueryCollection collections);
        PaypalResponseModel PaymentExecute(IQueryCollection collections);
    }
}