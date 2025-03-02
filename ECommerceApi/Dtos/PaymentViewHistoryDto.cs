namespace ECommerceApi.Dtos
{
    public class PaymentViewHistoryDto
    {
        public string ProductPay { get; set; }
        public string PaymentGateway { get; set; }
        public long ProductPrice { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
