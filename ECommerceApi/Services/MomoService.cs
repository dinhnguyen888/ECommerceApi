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
    public MomoService( IConfiguration configuration)
    {
     
        _configuration = configuration;
        Endpoint = _configuration["Momo:Endpoint"];
        PartnerCode = _configuration["Momo:PartnerCode"];
        AccessKey = _configuration["Momo:AccessKey"];
        SecretKey = _configuration["Momo:SecretKey"];
        RedirectUrl = _configuration["URL:FrontendUrlPaymentCallback"];
        IpnUrl = _configuration["Momo:NotifyUrl"];

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
