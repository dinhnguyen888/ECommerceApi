namespace ECommerceApi.Dtos
{
    public class PaymentResult
    {
        public long OrderId { get; set; }
        public long TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
        public long Amount { get; set; }
        public string BankCode { get; set; }
        public string TerminalId { get; set; }
        public bool IsValidSignature { get; set; }
        public bool IsError { get; set; }
    }
}
