using System.ComponentModel.DataAnnotations;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO de tao thanh toan (init payment)
    public class PaymentCreateDto
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "So tien phai lon hon 0")]
        public decimal Amount { get; set; }
    }
}
