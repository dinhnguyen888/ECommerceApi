using ECommerceApi.Dtos;
using VNPAY.NET.Models;

namespace ECommerceApi.Interfaces
{
    public interface IVnpayService
    {
        Task<string> CreatePaymentInVnpay(PaymentPostDto dto, string ipAddress);
        string CreatePaymentUrl(long moneyToPay, string description, string ipAddress);
        Task<bool> handleIpnCallBack(IQueryCollection query);
        PaymentResult ProcessPaymentResult(IQueryCollection query);
    }
}