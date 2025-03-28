﻿using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : Controller
    {
        private readonly IPayPalService _payPalService;
        private readonly IConfiguration _configuration;

        public PayPalController(IPayPalService payPalService, IConfiguration configuration)
        {
            _payPalService = payPalService;
            _configuration = configuration;
        }

        [HttpPost("create-paypal-payment")]
        public async Task<IActionResult> CreatePaymentUrl([FromQuery]PaymentPostDto dto)
        {
            try
            {
                var payUrl = await _payPalService.CreatePaymentInPayPal(dto);
                return Created(payUrl, payUrl);
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
                return Redirect(_configuration["URL:FrontendUrlPaymentCallback"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] PaymentCallback: {ex.Message}");
                return StatusCode(500, new { message = "Error processing PayPal callback", error = ex.Message });
            }
        }
    }
}
