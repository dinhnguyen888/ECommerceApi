namespace ECommerceApi.Dtos
{
    public class PaymentUpdateDto  
    {
        public string ProductPay { get; set; }
        public Guid? UserId { get; set; }
        public string PaymentGateway { get; set; }
        public long ProductPrice { get; set; }
        public DateTime PaymentDate { get; set; }
   
    }
}
