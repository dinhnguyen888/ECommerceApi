namespace ECommerceApi.Dtos
{
    using VNPAY.NET;
    using VNPAY.NET.Enums; // Đảm bảo bạn đã thêm thư viện đúng cách

    public class PaymentRequest  
    {
        public long PaymentId { get; set; } // Mã thanh toán
        public double Money { get; set; } // Số tiền cần thanh toán
        public string Description { get; set; } // Mô tả giao dịch
        public string IpAddress { get; set; } // Địa chỉ IP
        public BankCode BankCode { get; set; } // Mã ngân hàng (enum từ VNPAY.NET)
        public DateTime CreatedDate { get; set; } // Ngày tạo giao dịch
        public Currency Currency { get; set; } // Đơn vị tiền tệ (enum từ VNPAY.NET)
        public DisplayLanguage Language { get; set; } // Ngôn ngữ hiển thị (enum từ VNPAY.NET)
    }



}
