namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO de nhan callback tu VNPay sau khi thanh toan
    public class VnpayCallbackDto
    {
        public string vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_BankTranNo { get; set; }
        public string vnp_CardType { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_PayDate { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_SecureHash { get; set; }
    }

    // DTO de tra ve URL thanh toan VNPay
    public class VnpayPaymentUrlDto
    {
        public string PaymentUrl { get; set; }
        public string TransactionId { get; set; }
    }
}
