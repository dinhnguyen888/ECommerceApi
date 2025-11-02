using System;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;

namespace ECommerce.TransactionService.Application.Services
{
    // Service xu ly business logic cua Payment
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IVnpayService _vnpayService;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IOrderRepository orderRepo,
            IVnpayService vnpayService,
            IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _vnpayService = vnpayService;
            _mapper = mapper;
        }

        // Tao thanh toan va tra ve URL thanh toan VNPay
        public async Task<VnpayPaymentUrlDto> CreatePaymentAsync(PaymentCreateDto dto)
        {
            // Kiem tra don hang ton tai
            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null)
                throw new ArgumentException($"Don hang khong ton tai: {dto.OrderId}");

            // Kiem tra phuong thuc thanh toan
            if (dto.PaymentMethod != PaymentMethod.Vnpay)
                throw new ArgumentException("Chi ho tro thanh toan qua VNPay");

            // Tao transaction ID
            var transactionId = Guid.NewGuid().ToString();

            // Tao Payment entity
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = dto.OrderId,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = transactionId,
                Amount = dto.Amount,
                PaidAt = null // Chua thanh toan
            };

            // Luu payment vao database
            await _paymentRepo.CreateAsync(payment);

            // Tao URL thanh toan VNPay
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

            // Kiem tra response code
            if (callback.vnp_ResponseCode != "00")
            {
                var errorMessage = _vnpayService.GetResponseMessage(callback.vnp_ResponseCode);
                throw new InvalidOperationException($"Thanh toan that bai: {errorMessage}");
            }

            // Kiem tra transaction status
            if (callback.vnp_TransactionStatus != "00")
            {
                throw new InvalidOperationException("Giao dich khong thanh cong");
            }

            // Tim payment theo TransactionId (vnp_TxnRef)
            var payment = await _paymentRepo.GetByTransactionIdAsync(callback.vnp_TxnRef);
            if (payment == null)
            {
                throw new ArgumentException($"Khong tim thay payment voi transactionId: {callback.vnp_TxnRef}");
            }

            // Neu da thanh toan roi thi khong cap nhat lai
            if (payment.PaidAt != null)
            {
                return true;
            }

            // Cap nhat thong tin thanh toan
            payment.PaidAt = DateTime.UtcNow;
            payment.TransactionId = callback.vnp_TransactionNo ?? payment.TransactionId;
            await _paymentRepo.UpdateAsync(payment);

            // Cap nhat trang thai don hang thanh Paid
            var order = await _orderRepo.GetByIdAsync(payment.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Paid;
                await _orderRepo.UpdateAsync(order);
            }

            return true;
        }

        // Lay thong tin thanh toan theo ID
        public async Task<PaymentGetDto?> GetPaymentByIdAsync(string id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null)
                return null;

            return _mapper.Map<PaymentGetDto>(payment);
        }

        // Lay thong tin thanh toan theo TransactionId
        public async Task<PaymentGetDto?> GetPaymentByTransactionIdAsync(string transactionId)
        {
            var payment = await _paymentRepo.GetByTransactionIdAsync(transactionId);
            if (payment == null)
                return null;

            return _mapper.Map<PaymentGetDto>(payment);
        }
    }
}
