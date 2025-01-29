using ECommerceApi.Models;
using System.Text.Json.Serialization;

namespace ECommerceApi.Dtos
{
    public class PaymentPostDto
    {
        public string ProductPay { get; set; }
        public string ProductId { get; set; }
        public Guid UserId { get; set; }
        public string PaymentGateway { get; set; }
        public long ProductPrice { get; set; }
    }
}
