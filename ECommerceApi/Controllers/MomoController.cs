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
    private readonly IConfiguration _configuration;

    public MomoController(MomoService momoService, IPaymentService paymentService, ILogger<MomoController> logger, IConfiguration configuration)
    {
        _momoService = momoService;
        _paymentService = paymentService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("create-payment")]
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
                return Ok(payUrl);
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
    public async Task<IActionResult> IpnAction([FromBody] MomoIpnRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.OrderId))
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            // Validate the signature
            //string secretKey = _configuration["Momo:SecretKey"];
            //string rawData = $"{request.PartnerCode}{request.OrderId}{request.RequestId}{request.Amount}{request.OrderInfo}{request.OrderType}{request.TransId}{request.ResultCode}{request.Message}{request.PayType}{request.ResponseTime}{request.ExtraData}";
            ////string calculatedSignature = _momoService.CreateSignature(rawData, secretKey);

            //if (request.Signature != calculatedSignature)
            //{
            //    _logger.LogWarning("Invalid signature for Momo IPN request.");
            //    return BadRequest(new { message = "Invalid signature." });
            //}

            var payment = await _paymentService.ChangePaymentStatusAndGetPaymentInfo(request.ResultCode == 0, long.Parse(request.OrderId));
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            await _paymentService.SendEmailUsingPaymentInfo(payment);

            // Trả về phản hồi chuẩn của MoMo
            return Ok(
          );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing Momo IPN.");
            return StatusCode(500, new { message = "An error occurred while processing the IPN request." });
        }
    }

}


