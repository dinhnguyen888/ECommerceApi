using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO de tao don hang buy-now
    public class BuyNowDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "So luong phai lon hon 0")]
        public int Quantity { get; set; } = 1;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Gia phai lon hon 0")]
        public decimal Price { get; set; }
    }
}
