namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO tra ve ket qua buy-now
    public class BuyNowResponseDto
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string PaymentUrl { get; set; } // VNPay payment URL
        public string TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
