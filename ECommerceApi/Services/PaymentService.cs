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

        public PaymentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<PaymentGetDto> CreatePaymentAsync(PaymentPostDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return _mapper.Map<PaymentGetDto>(payment);
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

        public string ConvertPaymentInfoToDescriptionInVnpay(PaymentPostDto payment)
        {
            var productName = payment.ProductPay;
            var userId = payment.UserId;
            var price = payment.ProductPrice;
            var paymentGateway = "VNPAY";

            return $"{productName} {userId} {price} {paymentGateway}";
        }

        public async Task<bool> SplitDescriptionAndCreatePayment(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty", nameof(description));
            }

            string[] descriptionArray = description.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // Validate that the description has enough parts
            if (descriptionArray.Length < 4)
            {
                throw new ArgumentException("Description must contain at least four parts: ProductName, UserId, Price, and PaymentGateway.");
            }

            string productName = descriptionArray[0];
            string userId = descriptionArray[1];
            string priceString = descriptionArray[2];
            string paymentGateway = descriptionArray[3];

            // Parse userId to Guid
            Guid? userGuid = Guid.TryParse(userId, out var parsedGuid) ? parsedGuid : null;
            if (userGuid == null)
            {
                throw new ArgumentException("UserId must be a valid GUID.");
            }

            // Parse price to decimal (or another appropriate type)
            if (!double.TryParse(priceString, out var productPrice) || productPrice <= 0)
            {
                throw new ArgumentException("Price must be a valid positive decimal number.");
            }

            // Create the payment DTO
            var payment = new PaymentPostDto
            {
                ProductPay = productName,
                UserId = userGuid,
                ProductPrice = productPrice,
                PaymentGateway = paymentGateway
            };

            // Call the CreatePaymentAsync method
            var createPayment = await this.CreatePaymentAsync(payment);
            if (createPayment == null)
            {
                throw new Exception("Failed to create payment.");
            }

            return true;
        }

    }
}
