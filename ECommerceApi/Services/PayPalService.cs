using System.Net;
using System.Security.AccessControl;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using PayPal.Core;
using PayPal.v1.Payments;

namespace ECommerceApi.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;
        private readonly AppDbContext _context;
        private const double ExchangeRate = 22_863.0;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayPalService(IConfiguration configuration, IPaymentService paymentService, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public static double ConvertVndToDollar(double vnd)
        {
            var total = Math.Round(vnd / ExchangeRate, 2);

            return total;
        }

        public async Task<string> CreatePaymentInPayPal(PaymentPostDto dto)
        {
           using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var paymentId = await _paymentService.CreatePaymentAsync(dto);
                    var paymentUrl = await CreatePaymentUrl(paymentId, dto.ProductPrice, dto.ProductId, dto.ProductPay);
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

        public async Task PaypalCallbackHandle(IQueryCollection collections)
        {
            try
            {
                // check parameters from PayPal callback
                if (!collections.ContainsKey("payment_method") ||
                    !collections.ContainsKey("success") ||
                    !collections.ContainsKey("order_id"))
                {
                    throw new ArgumentException("Invalid PayPal callback parameters");
                }

                // call PaymentExecute to get response
                var response = this.PaymentExecute(collections);
                if (response == null)
                {
                    throw new Exception("PaymentExecute failed: Response is null");
                }

                // transform order ID to long
                if (!long.TryParse(response.OrderId, out var orderId))
                {
                    throw new ArgumentException("Invalid order ID");
                }

                // Change payment status and get payment info
                var paymentInfo = await _paymentService.ChangePaymentStatusAndGetPaymentInfo(true, orderId);
                if (paymentInfo == null)
                {
                    throw new Exception($"Payment info not found for order ID: {orderId}");
                }

                // send email using payment info
                await _paymentService.SendEmailUsingPaymentInfo(paymentInfo);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"PayPal callback error: {ex.Message}");
                throw;
            }
        }


        public async Task<string> CreatePaymentUrl(string paymentId, double price, string productId, string productName)
        {
            var envProd = new LiveEnvironment(_configuration["Paypal:ClientId"], _configuration["Paypal:SecretKey"]);
            var client = new PayPalHttpClient(envProd);
            var paypalOrderId = paymentId;
            var baseRequest = _httpContextAccessor.HttpContext?.Request;

            if (baseRequest == null)
            {
                throw new InvalidOperationException("HttpContext or Request is null");
            }

            var urlCallBack = $"{baseRequest.Scheme}://{baseRequest.Host}/api/Paypal/paypal-callback";
            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = ConvertVndToDollar(price).ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = ConvertVndToDollar(price).ToString(),
                            }
                        },
                        ItemList = new ItemList()
                        {
                            Items = new List<Item>()
                            {
                                new Item()
                                {
                                    Name = " | Order: " + productName,
                                    Currency = "USD",
                                    Price = ConvertVndToDollar(price).ToString(),
                                    Quantity = 1.ToString(),
                                    Sku = "sku",
                                    Tax = "0",
                                    Url = _configuration["URL:FrontendUrl"],
                                }
                            }
                        },
                        Description = $"Invoice #{productId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                    CancelUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            var request = new PaymentCreateRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
                return paymentUrl;

            var result = response.Result<Payment>();
            using var links = result.Links.GetEnumerator();

            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }

            return paymentUrl;
        }

     

        public PaypalResponseModel PaymentExecute(IQueryCollection collections)
        {
            var response = new PaypalResponseModel();

            foreach (var (key, value) in collections)
            {
               
                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_id"))
                {
                    response.OrderId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_method"))
                {
                    response.PaymentMethod = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("success"))
                {
                    response.Success = Convert.ToInt32(value) > 0;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("paymentId"))
                {
                    response.PaymentId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("PayerId"))
                {
                    response.PayerId = value;
                }
            }

            return response;
        }
    }
}