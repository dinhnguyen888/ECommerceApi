using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq.Expressions;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;

namespace ECommerceApi.Services
{
    public class VnpayService : IVnpayService
    {
        private readonly IVnpay _vnpay;
        private readonly string _tmnCode;
        private readonly string _hashSecret;
        private readonly string _baseUrl;
        private readonly string _callbackUrl;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IMongoCollection<Product> _productCollection;

        public VnpayService(MongoDbContext dbContext, IConfiguration configuration, IPaymentService paymentService, IEmailService emailService, AppDbContext context, IMapper mapper, IProductService productService)
        {
            _vnpay = new Vnpay();
            _tmnCode = configuration["Vnpay:TmnCode"];
            _hashSecret = configuration["Vnpay:HashSecret"];
            _baseUrl = configuration["Vnpay:BaseUrl"];
            _callbackUrl = configuration["Vnpay:ReturnUrl"];
            _vnpay.Initialize(_tmnCode, _hashSecret, _baseUrl, _callbackUrl);
            _paymentService = paymentService;
            _emailService = emailService;
            _context = context;
            _mapper = mapper;
            _productCollection = dbContext.GetCollection<Product>("Product");
            _productService = productService;
        }

        public async Task<string> CreatePaymentInVnpay(PaymentPostDto dto, string ipAddress)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var paymentId = await _paymentService.CreatePaymentAsync(dto);
                    var paymentUrl = this.CreatePaymentUrl(dto.ProductPrice, paymentId, ipAddress);
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

        public async Task<bool> handleIpnCallBack(IQueryCollection query)
        {
            var paymentResult = _vnpay.GetPaymentResult(query);
            if (paymentResult.IsSuccess)
            {
                var description = paymentResult.Description;
                if (!long.TryParse(description, out var paymentId))
                {
                    throw new InvalidOperationException("Invalid payment ID in the description.");
                }
                var payment = await _paymentService.ChangePaymentStatusAndGetPaymentInfo(true, paymentId);
                await _paymentService.SendEmailUsingPaymentInfo(payment);
                return true;
            }
            return false;
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
