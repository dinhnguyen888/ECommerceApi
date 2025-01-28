using ECommerceApi.Models;

namespace ECommerceApi.Dtos
{
    public class PaymentPostDto
    {
        public string ProductPay { get; set; }
        public Guid? UserId { get; set; }
        public string PaymentGateway { get; set; }
        public double ProductPrice { get; set; }
    }
}
