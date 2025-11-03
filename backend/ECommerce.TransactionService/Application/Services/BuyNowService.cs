using System;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Application.Events;
using ECommerce.TransactionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.TransactionService.Application.Services
{
    public class BuyNowService : IBuyNowService
    {
        private readonly TransactionDbContext _db;
        private readonly IVnpayService _vnpayService;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public BuyNowService(
            TransactionDbContext db,
            IVnpayService vnpayService,
            IMapper mapper,
            IMessagePublisher messagePublisher)
        {
            _db = db;
            _vnpayService = vnpayService;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<BuyNowResponseDto> BuyNowAsync(BuyNowDto dto)
        {
            // Bat dau transaction
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Tinh tong tien
                var totalAmount = dto.Price * dto.Quantity;

                // Step 2: Tao order
                var order = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = dto.UserId,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(30) // Het han sau 30 phut
                };

                // Tao order item
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = order.Id,
                    ProductId = dto.ProductId,
                    Price = dto.Price,
                    Quantity = dto.Quantity
                };
                order.OrderItems.Add(orderItem);

                // Luu order
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                // Step 3: Tao payment record
                var transactionId = Guid.NewGuid().ToString();
                var payment = new Payment
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = order.Id,
                    PaymentMethod = PaymentMethod.Vnpay,
                    Status = PaymentStatus.Pending,
                    TransactionId = transactionId,
                    Amount = totalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PaidAt = null
                };

                _db.Payments.Add(payment);
                await _db.SaveChangesAsync();

                // Step 4: Tao VNPay payment URL
                var paymentUrl = await _vnpayService.CreatePaymentUrlAsync(
                    order.Id,
                    totalAmount,
                    transactionId);

                // Commit transaction
                await transaction.CommitAsync();

                // Publish order created event
                var orderCreatedEvent = new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    CreatedAt = order.CreatedAt
                };
                _messagePublisher.PublishToQueue("order.created", orderCreatedEvent);

                // Tra ve ket qua
                return new BuyNowResponseDto
                {
                    OrderId = order.Id,
                    PaymentId = payment.Id,
                    PaymentUrl = paymentUrl,
                    TransactionId = transactionId,
                    TotalAmount = totalAmount
                };
            }
            catch (Exception)
            {
                // Rollback transaction neu co loi
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
