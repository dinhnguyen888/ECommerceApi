using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VNPAY.NET.Models;

[ApiController]
[Route("api/Momo")]
public class MomoController : ControllerBase
{
    private readonly MomoService _momoService;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<MomoController> _logger;

    public MomoController(MomoService momoService, IPaymentService paymentService, ILogger<MomoController> logger)
    {
        _momoService = momoService;
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment([FromQuery] PaymentPostDto request)
    {
        try
        {
            var paymentId = await _paymentService.CreatePaymentAsync(request);
            long amount = request.ProductPrice;
            string description = request.ProductPay;
            string orderId = paymentId.ToString();

            var payUrl = await _momoService.CreatePaymentRequestAsync(amount, description, orderId);
            if (!string.IsNullOrEmpty(payUrl))
            {
                return Ok(new { payUrl });
            }

            return BadRequest("Failed to create a payment request.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating Momo payment request.");
            return StatusCode(500, "An error occurred while processing the payment request.");
        }
    }

    [HttpPost("ipn")]
    public async Task<IActionResult> IpnAction([FromBody] MomoIpnDto ipnRequest)
    {
        try
        {
            if (ipnRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            var paymentId = Convert.ToInt64(ipnRequest.OrderId);

            var payment = await _paymentService.ChangePaymentStatusAndGetPaymentInfo(true, paymentId);
            await _paymentService.SendEmailUsingPaymentInfo(payment);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing Momo IPN.");
            return StatusCode(500, "An error occurred while processing the IPN request.");
        }
    }
}
