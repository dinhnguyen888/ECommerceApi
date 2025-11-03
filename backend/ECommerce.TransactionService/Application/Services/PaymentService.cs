using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Application.Events;

namespace ECommerce.TransactionService.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IVnpayService _vnpayService;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IOrderRepository orderRepo,
            IVnpayService vnpayService,
            IMapper mapper,
            IMessagePublisher messagePublisher)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _vnpayService = vnpayService;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<VnpayPaymentUrlDto> CreatePaymentAsync(PaymentCreateDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null)
                throw new ArgumentException($"Don hang khong ton tai: {dto.OrderId}");

            if (dto.PaymentMethod != PaymentMethod.Vnpay)
                throw new ArgumentException("Chi ho tro thanh toan qua VNPay");

            var transactionId = Guid.NewGuid().ToString();

            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = dto.OrderId,
                PaymentMethod = dto.PaymentMethod,
                Status = PaymentStatus.Pending,
                TransactionId = transactionId,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PaidAt = null
            };

            await _paymentRepo.CreateAsync(payment);

            var paymentUrl = await _vnpayService.CreatePaymentUrlAsync(
                dto.OrderId,
                dto.Amount,
                transactionId);

            return new VnpayPaymentUrlDto
            {
                PaymentUrl = paymentUrl,
                TransactionId = transactionId
            };
        }

        // Xu ly callback tu VNPay sau khi thanh toan
        public async Task<bool> HandleVnpayCallbackAsync(VnpayCallbackDto callback)
        {
            // Xac thuc chu ky
            if (!_vnpayService.ValidateCallbackSignature(callback, callback.vnp_SecureHash))
            {
                throw new UnauthorizedAccessException("Chu ky khong hop le");
            }

            // Tim payment theo TransactionId (vnp_TxnRef)
            var payment = await _paymentRepo.GetByTransactionIdAsync(callback.vnp_TxnRef);
            if (payment == null)
            {
                throw new ArgumentException($"Khong tim thay payment voi transactionId: {callback.vnp_TxnRef}");
            }

            // Neu da thanh toan roi thi khong cap nhat lai
            if (payment.PaidAt != null && payment.Status == PaymentStatus.Completed)
            {
                return true;
            }

            // Kiem tra response code va transaction status
            if (callback.vnp_ResponseCode != "00" || callback.vnp_TransactionStatus != "00")
            {
                // Thanh toan that bai
                var errorMessage = _vnpayService.GetResponseMessage(callback.vnp_ResponseCode);
                
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = errorMessage;
                payment.UpdatedAt = DateTime.UtcNow;
                await _paymentRepo.UpdateAsync(payment);

                // Cap nhat trang thai don hang thanh Failed
                var order = await _orderRepo.GetByIdAsync(payment.OrderId);
                if (order != null)
                {
                    order.Status = OrderStatus.Failed;
                    order.UpdatedAt = DateTime.UtcNow;
                    await _orderRepo.UpdateAsync(order);

                    // Publish payment failed event
                    var paymentFailedEvent = new PaymentFailedEvent
                    {
                        PaymentId = payment.Id,
                        OrderId = payment.OrderId,
                        UserId = order.UserId,
                        Amount = payment.Amount,
                        FailureReason = errorMessage,
                        FailedAt = DateTime.UtcNow
                    };
                    _messagePublisher.PublishToQueue("payment.failed", paymentFailedEvent);
                }

                return false;
            }

            // Thanh toan thanh cong
            payment.Status = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;
            payment.TransactionId = callback.vnp_TransactionNo ?? payment.TransactionId;
            payment.UpdatedAt = DateTime.UtcNow;
            await _paymentRepo.UpdateAsync(payment);

            // Cap nhat trang thai don hang thanh Paid
            var successOrder = await _orderRepo.GetByIdAsync(payment.OrderId);
            if (successOrder != null)
            {
                successOrder.Status = OrderStatus.Paid;
                successOrder.UpdatedAt = DateTime.UtcNow;
                await _orderRepo.UpdateAsync(successOrder);

                // Publish payment completed event (cho notification service)
                var paymentCompletedEvent = new PaymentCompletedEvent
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    UserId = successOrder.UserId,
                    Amount = payment.Amount,
                    PaidAt = payment.PaidAt ?? DateTime.UtcNow
                };
                _messagePublisher.PublishToQueue("payment.completed", paymentCompletedEvent);

                // Publish payment success notification event (de gui mail va thong bao)
                var paymentSuccessNotificationEvent = new PaymentSuccessNotificationEvent
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    UserId = successOrder.UserId,
                    Amount = payment.Amount,
                    PaidAt = payment.PaidAt ?? DateTime.UtcNow
                };
                _messagePublisher.PublishToQueue("payment.success.notification", paymentSuccessNotificationEvent);
            }

            return true;
        }

        public async Task<PaymentGetDto?> GetPaymentByIdAsync(string id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null)
                return null;

            return _mapper.Map<PaymentGetDto>(payment);
        }

        public async Task<PaymentGetDto?> GetPaymentByTransactionIdAsync(string transactionId)
        {
            var payment = await _paymentRepo.GetByTransactionIdAsync(transactionId);
            if (payment == null)
                return null;

            return _mapper.Map<PaymentGetDto>(payment);
        }

        public async Task<List<PaymentGetDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepo.GetAllAsync();
            return _mapper.Map<List<PaymentGetDto>>(payments);
        }

        public async Task<List<PaymentGetDto>> GetPaymentsByOrderIdAsync(string orderId)
        {
            var payments = await _paymentRepo.GetByOrderIdAsync(orderId);
            return _mapper.Map<List<PaymentGetDto>>(payments);
        }

        public async Task<PaymentGetDto> UpdatePaymentAsync(PaymentUpdateDto dto)
        {
            var payment = await _paymentRepo.GetByIdAsync(dto.Id);
            if (payment == null)
                throw new ArgumentException($"Thanh toan khong ton tai: {dto.Id}");

            if (dto.PaymentMethod.HasValue)
                payment.PaymentMethod = dto.PaymentMethod.Value;

            if (!string.IsNullOrEmpty(dto.TransactionId))
                payment.TransactionId = dto.TransactionId;

            if (dto.PaidAt.HasValue)
                payment.PaidAt = dto.PaidAt.Value;

            if (dto.Amount.HasValue)
                payment.Amount = dto.Amount.Value;

            await _paymentRepo.UpdateAsync(payment);

            var updatedPayment = await _paymentRepo.GetByIdAsync(dto.Id);
            return _mapper.Map<PaymentGetDto>(updatedPayment!);
        }

        public async Task DeletePaymentAsync(string id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null)
                throw new ArgumentException($"Thanh toan khong ton tai: {id}");

            await _paymentRepo.DeleteAsync(id);
        }
    }
}
