using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ECommerce.TransactionService.Application.Services
{
    // Service xu ly VNPay payment
    // LUU Y: Service nay duoc cau hinh cho SANDBOX environment
    // Neu can su dung Production, can thay doi URL va su dung TmnCode/HashSecret tu VNPay production
    public class VnpayService : IVnpayService
    {
        private readonly IConfiguration _config;
        private readonly string _tmCode;
        private readonly string _hashSecret;
        private readonly string _url;
        private readonly string _returnUrl;
        private readonly string _ipnUrl;

        public VnpayService(IConfiguration config)
        {
            _config = config;
            _tmCode = _config["Vnpay:TmnCode"] ?? string.Empty;
            _hashSecret = _config["Vnpay:HashSecret"] ?? string.Empty;
            
            // Mac dinh su dung Sandbox URL
            // Production URL: https://www.vnpayment.vn/paymentv2/vpcpay.html
            _url = _config["Vnpay:Url"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            _returnUrl = _config["Vnpay:ReturnUrl"] ?? string.Empty;
            _ipnUrl = _config["Vnpay:IpnUrl"] ?? string.Empty;
        }

        // Tao URL thanh toan VNPay
        public async Task<string> CreatePaymentUrlAsync(string orderId, decimal amount, string transactionId)
        {
            return await Task.Run(() =>
            {
                // Chuyen doi amount sang VND (VNPay yeu cau so tien bang VND, nhan voi 100)
                var amountInVnd = (long)(amount * 100);

                // Tao query string parameters cho VNPay
                var vnpParams = new Dictionary<string, string>
                {
                    { "vnp_Version", "2.1.0" },
                    { "vnp_Command", "pay" },
                    { "vnp_TmnCode", _tmCode },
                    { "vnp_Amount", amountInVnd.ToString() },
                    { "vnp_CurrCode", "VND" },
                    { "vnp_TxnRef", transactionId },
                    { "vnp_OrderInfo", $"Thanh toan don hang {orderId}" },
                    { "vnp_OrderType", "other" },
                    { "vnp_Locale", "vn" },
                    { "vnp_ReturnUrl", _returnUrl },
                    { "vnp_IpAddr", "127.0.0.1" } // IP cua client, co the lay tu request
                };

                // Them IPN URL neu co (server-to-server callback)
                if (!string.IsNullOrEmpty(_ipnUrl))
                {
                    vnpParams.Add("vnp_IpnUrl", _ipnUrl);
                }

                vnpParams.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

                // Sap xep theo key de tao chu ky (bo qua vnp_SecureHash)
                var sortedParams = vnpParams.OrderBy(x => x.Key)
                    .Where(x => !x.Key.Equals("vnp_SecureHash", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                // Tao query string khong co vnp_SecureHash
                var queryString = string.Join("&", sortedParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

                // Tao chu ky hash (khong can them hashSecret vao cuoi vi HMAC da co secret)
                var secureHash = ComputeHash(queryString);

                // Them chu ky vao query string
                queryString += $"&vnp_SecureHash={secureHash}";

                // Tra ve URL day du
                return $"{_url}?{queryString}";
            });
        }

        // Xac thuc chu ky tu callback VNPay
        public bool ValidateCallbackSignature(VnpayCallbackDto callback, string secureHash)
        {
            // Tao dictionary tu callback
            var vnpParams = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(callback.vnp_Amount))
                vnpParams.Add("vnp_Amount", callback.vnp_Amount);
            if (!string.IsNullOrEmpty(callback.vnp_BankCode))
                vnpParams.Add("vnp_BankCode", callback.vnp_BankCode);
            if (!string.IsNullOrEmpty(callback.vnp_BankTranNo))
                vnpParams.Add("vnp_BankTranNo", callback.vnp_BankTranNo);
            if (!string.IsNullOrEmpty(callback.vnp_CardType))
                vnpParams.Add("vnp_CardType", callback.vnp_CardType);
            if (!string.IsNullOrEmpty(callback.vnp_OrderInfo))
                vnpParams.Add("vnp_OrderInfo", callback.vnp_OrderInfo);
            if (!string.IsNullOrEmpty(callback.vnp_PayDate))
                vnpParams.Add("vnp_PayDate", callback.vnp_PayDate);
            if (!string.IsNullOrEmpty(callback.vnp_ResponseCode))
                vnpParams.Add("vnp_ResponseCode", callback.vnp_ResponseCode);
            if (!string.IsNullOrEmpty(callback.vnp_TmnCode))
                vnpParams.Add("vnp_TmnCode", callback.vnp_TmnCode);
            if (!string.IsNullOrEmpty(callback.vnp_TransactionNo))
                vnpParams.Add("vnp_TransactionNo", callback.vnp_TransactionNo);
            if (!string.IsNullOrEmpty(callback.vnp_TransactionStatus))
                vnpParams.Add("vnp_TransactionStatus", callback.vnp_TransactionStatus);
            if (!string.IsNullOrEmpty(callback.vnp_TxnRef))
                vnpParams.Add("vnp_TxnRef", callback.vnp_TxnRef);

            // Sap xep va tao query string (khong bao gom vnp_SecureHash)
            var sortedParams = vnpParams.OrderBy(x => x.Key)
                .Where(x => !x.Key.Equals("vnp_SecureHash", StringComparison.OrdinalIgnoreCase))
                .ToList();
            var queryString = string.Join("&", sortedParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

            // Tinh toan hash (HMAC SHA512)
            var computedHash = ComputeHash(queryString);

            // So sanh chu ky
            return computedHash.Equals(secureHash, StringComparison.OrdinalIgnoreCase);
        }

        // Giai ma response code tu VNPay
        public string GetResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dich thanh cong",
                "07" => "Trung ma tham chieu (transaction ID)",
                "09" => "The/Khong du so du",
                "10" => "Xac thuc khoa bi sai",
                "11" => "Da het han cho giao dich",
                "12" => "The bi khoa",
                "13" => "Nhap sai mat khau qua so lan cho phep",
                "51" => "Tai khoan khong du so du",
                "65" => "Tai khoan vuot qua han muc giao dich trong ngay",
                "75" => "Ngan hang thanh toan dang bao tri",
                "79" => "Nhap sai mat khau the qua so lan cho phep",
                "99" => "Loi khac",
                _ => $"Ma loi khong xac dinh: {responseCode}"
            };
        }

        // Tinh toan hash HMAC SHA512 (VNPay su dung HMAC SHA512)
        private string ComputeHash(string input)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_hashSecret));
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = hmac.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
