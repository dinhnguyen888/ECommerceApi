using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IProductService _productService;
        private readonly ITokenService _tokenService;

        public PaymentService(AppDbContext context,
            IMapper mapper,
            IEmailService emailService,
            IProductService productService,
            ITokenService tokenService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _productService = productService;
            _tokenService = tokenService;
        }

        public async Task<(List<PaymentGetDto> payments, int totalPayments)> GetPaymentsAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var totalPayments = await _context.Payments.CountAsync(); // Đếm tổng số bản ghi
            var payments = await _context.Payments
                                         .Skip(skip)
                                         .Take(pageSize)
                                         .ToListAsync();

            return (_mapper.Map<List<PaymentGetDto>>(payments), totalPayments);
        }


        public async Task<IEnumerable<PaymentViewHistoryDto>> ViewPaymentHistory(string token)
        {
            Guid accountId = Guid.Parse(_tokenService.ValidateTokenAndGetUserId(token));
            var payments = await _context.Payments.Where(p => p.UserId == accountId).ToListAsync();
            return _mapper.Map<IEnumerable<PaymentViewHistoryDto>>(payments);
        }

        public async Task<string> CreatePaymentAsync(PaymentPostDto paymentDto)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == paymentDto.UserId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }
            var payment = _mapper.Map<Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.UserId = account.Id;
            payment.PaymentStatus = false;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment.Id.ToString();


        }

        public async Task<PaymentGetDto> UpdatePaymentAsync(Guid id, PaymentUpdateDto paymentDto)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                throw new KeyNotFoundException("Payment not found");
            }

            _mapper.Map(paymentDto, payment);
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return _mapper.Map<PaymentGetDto>(payment);
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
            {
                return false;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePendingPaymentAsync()
        {
            var payments = await _context.Payments.Where(p => p.PaymentStatus == false).ToListAsync();
            if (payments.Count == 0)
            {
                return false;
            }
            _context.Payments.RemoveRange(payments);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaymentGetDto> ChangePaymentStatusAndGetPaymentInfo(bool paymentStatus, long paymentId)
        {
           
            // get payment
            var payment = await _context.Payments.SingleOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null) throw new ArgumentNullException("Payment not found");

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
                    Bạn vừa đặt mua phần mềm {paymentResult.ProductPay} với giá {paymentResult.ProductPrice} vào ngày {paymentResult.PaymentDate}.
                    Vui lòng click vào đường link bên dưới để tải:
                    {productURL}";



            // Log email details
            Console.WriteLine($"[LOG] Sending email to: {account.Email}, Subject: {subject}, Body: {body}");

            // Gửi email
            await _emailService.SendEmail(account.Email, subject, body);
        }
    }
}
