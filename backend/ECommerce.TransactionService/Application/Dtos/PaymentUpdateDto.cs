using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Dtos
{
    public class PaymentUpdateDto
    {
        [Required]
        public string Id { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public string TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }

        public decimal? Amount { get; set; }
    }
}
