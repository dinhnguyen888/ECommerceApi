namespace ECommerceApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string ProductPay { get; set; }
        public string ProductId { get; set; }
        public Guid? UserId { get; set; }
        public string PaymentGateway { get; set; }
        public long ProductPrice { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public Account Account { get; set; }
        public bool PaymentStatus { get; set; } = false;
    }
}
