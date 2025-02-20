using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalController : Controller
    {
        private readonly IPayPalService _payPalService;

        public PaypalController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        [HttpPost("create-paypal-payment")]
        public async Task<IActionResult> CreatePaymentUrl([FromQuery]PaymentPostDto dto)
        {
            try
            {
                var url = await _payPalService.CreatePaymentInPayPal(dto);
                return Ok(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreatePaymentUrl: {ex.Message}");
                return StatusCode(500, new { message = "Error creating PayPal payment", error = ex.Message });
            }
        }

        [HttpGet("paypal-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            try
            {
                await _payPalService.PaypalCallbackHandle(Request.Query);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] PaymentCallback: {ex.Message}");
                return StatusCode(500, new { message = "Error processing PayPal callback", error = ex.Message });
            }
        }
    }
}
