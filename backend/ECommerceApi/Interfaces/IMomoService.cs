using ECommerceApi.Dtos;

public interface IMomoService
{
    Task<string> CreatePaymentRequestAsync(long amount, string description, string orderIdInput);
    Task<string> CreatePaymentWithMomo(PaymentPostDto dto);
    string CreateSignature(string data, string key);
    Task<bool> HandleMomoIpn(MomoIpnRequest request);
}