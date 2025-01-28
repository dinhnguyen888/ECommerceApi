using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace ECommerceApi.Services
{
    public class VnpayService 
    {
        private readonly IVnpay _vnpay;
        private readonly string _tmnCode;
        private readonly string _hashSecret;
        private readonly string _baseUrl;
        private readonly string _callbackUrl;
        private readonly IPaymentService _paymentService;

        public VnpayService(IConfiguration configuration, IPaymentService paymentService)
        {
            _vnpay = new Vnpay();
            _tmnCode = configuration["Vnpay:TmnCode"];
            _hashSecret = configuration["Vnpay:HashSecret"];
            _baseUrl = configuration["Vnpay:BaseUrl"];
            _callbackUrl = configuration["Vnpay:ReturnUrl"];
            _vnpay.Initialize(_tmnCode, _hashSecret, _baseUrl, _callbackUrl);
            _paymentService = paymentService;

        }

        public string CreatePaymentUrl(long moneyToPay, string description, string ipAddress)
        {
            var request = new PaymentRequest
            {
                PaymentId = DateTime.Now.Ticks,
                Money = moneyToPay,
                Description = description,
                IpAddress = ipAddress,
                BankCode = BankCode.ANY,
                CreatedDate = DateTime.Now,
                Currency = Currency.VND,
                Language = DisplayLanguage.Vietnamese
            };

            return _vnpay.GetPaymentUrl(request);
        }

        public PaymentResult ProcessPaymentResult(IQueryCollection query)
        {
            
            return _vnpay.GetPaymentResult(query);
        }

       
    }
}
