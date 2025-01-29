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

        public PaymentService(AppDbContext context, IMapper mapper, IEmailService emailService, IProductService productService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _productService = productService;
        }

        public async Task<IEnumerable<PaymentGetDto>> GetAllPaymentsAsync()
        {
            var payments = await _context.Payments.ToListAsync();
            return _mapper.Map<IEnumerable<PaymentGetDto>>(payments);
        }

        public async Task<IEnumerable<PaymentGetDto>> GetPaymentsByAccountIdAsync(Guid accountId)
        {
            var payments = await _context.Payments.Where(p => p.UserId == accountId).ToListAsync();
            return _mapper.Map<IEnumerable<PaymentGetDto>>(payments);
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

        public async Task<bool> DeletePaymentAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return false;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
