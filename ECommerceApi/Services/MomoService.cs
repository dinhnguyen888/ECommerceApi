using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class MomoService : IMomoService
{
    private readonly string Endpoint;
    private readonly string PartnerCode;
    private readonly string AccessKey;
    private readonly string SecretKey;
    private readonly string RedirectUrl;
    private readonly string IpnUrl;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;
    private readonly IPaymentService _paymentService;
    public MomoService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, AppDbContext context, IPaymentService paymentService)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _context = context;
        _paymentService = paymentService;
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

    public async Task<string> CreatePaymentWithMomo(PaymentPostDto dto)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var paymentId = await _paymentService.CreatePaymentAsync(dto);
                var paymentUrl = await this.CreatePaymentRequestAsync(dto.ProductPrice, dto.ProductId, paymentId);
                await transaction.CommitAsync();
                return paymentUrl;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> HandleMomoIpn(MomoIpnRequest request)
    {
        var payment = await _paymentService.ChangePaymentStatusAndGetPaymentInfo(request.ResultCode == 0, long.Parse(request.OrderId));
        if (payment == null)
        {
            throw new Exception("something wrong when change PaymentStatus");
        }

        await _paymentService.SendEmailUsingPaymentInfo(payment);
        return true;
    }


    public async Task<string> CreatePaymentRequestAsync(long amount, string description, string orderIdInput)
    {
        string orderId = orderIdInput;
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
