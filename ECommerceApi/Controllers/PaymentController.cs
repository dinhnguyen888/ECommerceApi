using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<PaymentGetDto>>> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving payments.", Error = ex.Message });
            }
        }

        [HttpGet("/view-payment-history")]
        public async Task<ActionResult<IEnumerable<PaymentViewHistoryDto>>> ViewPaymentHistory([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                string cleanToken = token.Replace("Bearer ", "");
                var payments = await _paymentService.ViewPaymentHistory(cleanToken);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving payments by account ID.", Error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<PaymentGetDto>> CreatePayment(PaymentPostDto paymentDto)
        {
            try
            {
                 await _paymentService.CreatePaymentAsync(paymentDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the payment.", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<PaymentGetDto>> UpdatePayment(Guid id, PaymentUpdateDto paymentDto)
        {
            try
            {
                var payment = await _paymentService.UpdatePaymentAsync(id, paymentDto);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the payment.", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = "Payment not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the payment.", Error = ex.Message });
            }
        }
        [HttpDelete("pending-payment")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeletePendingPayment()
        {
           var result = await _paymentService.DeletePendingPaymentAsync();
            if (!result)
            {
                return NotFound(new { Message = "Payment not found." });
            }
            return NoContent();
        }

    }
}
