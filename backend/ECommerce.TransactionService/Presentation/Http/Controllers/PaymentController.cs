using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Interfaces;

namespace ECommerce.TransactionService.Presentation.Http.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PaymentGetDto>>> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<List<PaymentGetDto>>> GetPaymentsByOrderId(string orderId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaymentGetDto>> UpdatePayment(string id, [FromBody] PaymentUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { message = "ID trong URL va body khong khop" });

                var payment = await _paymentService.UpdatePaymentAsync(dto);
                return Ok(payment);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePayment(string id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }
    }
}
