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
    public class VnpayService 
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

        public async Task<PaymentGetDto> ChangePaymentStatusAndGetPaymentInfo(bool paymentStatus, long description)
        {
            //assign paymentId from description
            long paymentId = description;

            // get payment
            var payment = await _context.Payments.SingleOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null) throw new Exception("Payment not found");

            //Update payment status
            payment.PaymentStatus = paymentStatus;
            _context.Payments.Update(payment); 

            await _context.SaveChangesAsync(); 

            return _mapper.Map<PaymentGetDto>(payment);
        }


        public async Task SendEmailUsingPaymentInfo(PaymentGetDto paymentResult)
        {
            var productURL = await _productService.GetProductUrlByIdAsync(paymentResult.ProductId);
            if (productURL == null) throw new Exception("ProductId not found ");
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == paymentResult.UserId);
            if (account == null) throw new Exception("account not found");
            var subject = "Thanh toán thành công!! Phần mềm của bạn đã sẵn sàng.";
            var body = $@"
                    Bạn vừa đặt mua phần mềm {paymentResult.ProductPay} với giá {paymentResult.ProductPrice} vào ngày {paymentResult.PaymentDate}.\n
                    Vui lòng click vào đường link bên dưới để tải:\n
                    {productURL}";



            // Log email details
            Console.WriteLine($"[LOG] Sending email to: {account.Email}, Subject: {subject}, Body: {body}");

            // Gửi email
            await _emailService.SendEmail(account.Email, subject, body);
        }

    }
}
