using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Dtos;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho VNPay Service - xu ly logic VNPay
    public interface IVnpayService
    {
        // Tao URL thanh toan VNPay
        Task<string> CreatePaymentUrlAsync(string orderId, decimal amount, string transactionId);

        // Xac thuc chu ky tu callback VNPay
        bool ValidateCallbackSignature(VnpayCallbackDto callback, string secureHash);

        // Giai ma response code tu VNPay
        string GetResponseMessage(string responseCode);
    }
}
