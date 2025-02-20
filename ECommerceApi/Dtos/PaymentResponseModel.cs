namespace ECommerceApi.Dtos;

public class PaypalResponseModel
{
    public string OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentId { get; set; }
    public string PayerId { get; set; }
    public bool Success { get; set; }
}