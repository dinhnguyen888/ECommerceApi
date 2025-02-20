using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class MomoService
{
    private readonly string Endpoint;
    private readonly string PartnerCode;
    private readonly string AccessKey;
    private readonly string SecretKey;
    private readonly string RedirectUrl;
    private readonly string IpnUrl;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public MomoService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        // Lấy cấu hình
        Endpoint = _configuration.GetValue<string>("Momo:Endpoint");
        PartnerCode = _configuration.GetValue<string>("Momo:PartnerCode");
        AccessKey = _configuration.GetValue<string>("Momo:AccessKey");
        SecretKey = _configuration.GetValue<string>("Momo:SecretKey");
        RedirectUrl = _configuration.GetValue<string>("URL:FrontendUrlPaymentCallback");

        // Lấy thông tin từ HttpContext
        var baseRequest = _httpContextAccessor.HttpContext?.Request
                          ?? throw new InvalidOperationException("HttpContext or Request is null");

        // Tạo IpnUrl an toàn hơn
        var scheme = baseRequest.Scheme ?? "https";
        var host = baseRequest.Host.HasValue ? baseRequest.Host.Value : "localhost";
        IpnUrl = $"{scheme}://{host}/api/Momo/ipn";
    }
    public async Task<string> CreatePaymentRequestAsync(long amount, string description, string orderIdInput)
    {
        string orderId = orderIdInput;
        //string orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string requestId = orderId;
        string orderInfo = description;
        string requestType = "payWithATM";
        string extraData = "";
        Console.WriteLine(IpnUrl);
        // rawhash for create signature
        string rawHash = $"accessKey={AccessKey}&amount={amount}&extraData={extraData}&ipnUrl={IpnUrl}" +
                         $"&orderId={orderId}&orderInfo={orderInfo}&partnerCode={PartnerCode}" +
                         $"&redirectUrl={RedirectUrl}&requestId={requestId}&requestType={requestType}";


        // create signature HMAC SHA256
        string signature = CreateSignature(rawHash, SecretKey);

        var requestData = new
        {
            partnerCode = PartnerCode,
            partnerName = "Test",
            storeId = "MomoTestStore",
            requestId = requestId,
            amount = amount,
            orderId = orderId,
            orderInfo = orderInfo,
            redirectUrl = RedirectUrl,
            ipnUrl = IpnUrl,
            lang = "vi",
            extraData = extraData,
            requestType = requestType,
            signature = signature
        };

        using var client = new HttpClient();
        var jsonRequest = JsonSerializer.Serialize(requestData);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(Endpoint, content);
        string responseBody = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(responseBody);
        if (document.RootElement.TryGetProperty("payUrl", out var payUrl))
        {

            return payUrl.GetString();
        }

        return null;
    }

    public string CreateSignature(string data, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
