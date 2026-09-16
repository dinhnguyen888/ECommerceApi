using ECommerceApi.Dtos;
using Net.payOS.Types;

public interface IPayosService
{
    Task<PaymentLinkInformation> CancelOrderAsync(int orderId);
    Task ConfirmWebhookAsync(string webhookUrl);
    Task<string> CreatePaymentLink(PaymentPostDto body);
    Task<PaymentLinkInformation> GetOrderAsync(int orderId);
    Task<bool> ReceiveWebhook(WebhookType webhook);
}