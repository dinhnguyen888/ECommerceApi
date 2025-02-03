namespace ECommerceApi.Dtos
{
    public class MomoIpnDto
    {
        public string OrderId { get; set; }
        public long Amount { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }

}
