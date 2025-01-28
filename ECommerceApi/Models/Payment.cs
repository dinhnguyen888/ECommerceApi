namespace ECommerceApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string ProductPay { get; set; }
        public Guid? UserId { get; set; }
        public string PaymentGateway { get; set; }
        public string ProductPrice { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public Account Account { get; set; }
    }
}
