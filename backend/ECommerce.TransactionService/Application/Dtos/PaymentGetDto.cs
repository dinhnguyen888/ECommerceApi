using System;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO de tra ve thong tin thanh toan
    public class PaymentGetDto
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
    }
}
