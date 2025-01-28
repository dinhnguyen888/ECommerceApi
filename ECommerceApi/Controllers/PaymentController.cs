using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
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

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<PaymentGetDto>>> GetPaymentsByAccountId(Guid accountId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByAccountIdAsync(accountId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving payments by account ID.", Error = ex.Message });
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<PaymentGetDto>> CreatePayment(PaymentPostDto paymentDto)
        //{
        //    try
        //    {
        //        var payment = await _paymentService.CreatePaymentAsync(paymentDto);
        //        return CreatedAtAction(nameof(GetPaymentsByAccountId), new { accountId = payment.UserId }, payment);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "An error occurred while creating the payment.", Error = ex.Message });
        //    }
        //}

        [HttpPut("{id}")]
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
        public async Task<ActionResult> DeletePayment(Guid id)
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
    }
}
