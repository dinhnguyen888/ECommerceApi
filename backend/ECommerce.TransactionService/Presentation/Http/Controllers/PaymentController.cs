using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Interfaces;

namespace ECommerce.TransactionService.Presentation.Http.Controllers
{
    // Controller xu ly cac API lien quan den Payment
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST: api/payment/create
        // Tao thanh toan va tra ve URL thanh toan VNPay
        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<VnpayPaymentUrlDto>> CreatePayment([FromBody] PaymentCreateDto dto)
        {
            try
            {
                var result = await _paymentService.CreatePaymentAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        // POST: api/payment/vnpay-ipn
        // IPN (Instant Payment Notification) - VNPay goi ngam tu server de thong bao ket qua
        // Endpoint nay duoc VNPay goi tu server cua ho, khong phai tu browser cua user
        [HttpPost("vnpay-ipn")]
        [AllowAnonymous]
        public async Task<ActionResult> HandleVnpayIpn([FromQuery] VnpayCallbackDto callback)
        {
            try
            {
                var success = await _paymentService.HandleVnpayCallbackAsync(callback);
                if (success)
                {
                    // Tra ve ma thanh cong cho VNPay IPN
                    // VNPay se goi lai neu khong nhan duoc RspCode = "00"
                    return Ok(new { 
                        RspCode = "00", 
                        Message = "Success" 
                    });
                }
                return BadRequest(new { 
                    RspCode = "99", 
                    Message = "Failed" 
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { 
                    RspCode = "97", 
                    Message = ex.Message 
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { 
                    RspCode = "99", 
                    Message = ex.Message 
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { 
                    RspCode = "99", 
                    Message = ex.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    RspCode = "99", 
                    Message = "Loi he thong", 
                    Error = ex.Message 
                });
            }
        }

        // GET: api/payment/verify
        // Endpoint de frontend verify payment status sau khi nhan callback tu ReturnUrl
        [HttpGet("verify")]
        [AllowAnonymous]
        public async Task<ActionResult<PaymentGetDto>> VerifyPayment([FromQuery] string transactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    return BadRequest(new { message = "TransactionId la bat buoc" });
                }

                var payment = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);
                if (payment == null)
                {
                    return NotFound(new { message = $"Khong tim thay payment voi transactionId: {transactionId}" });
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        // GET: api/payment/{id}
        // Lay thong tin thanh toan theo ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PaymentGetDto>> GetPaymentById(string id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                    return NotFound(new { message = $"Khong tim thay thanh toan voi ID: {id}" });

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        // GET: api/payment/transaction/{transactionId}
        // Lay thong tin thanh toan theo TransactionId
        [HttpGet("transaction/{transactionId}")]
        [Authorize]
        public async Task<ActionResult<PaymentGetDto>> GetPaymentByTransactionId(string transactionId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);
                if (payment == null)
                    return NotFound(new { message = $"Khong tim thay thanh toan voi transactionId: {transactionId}" });

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }
    }
}
