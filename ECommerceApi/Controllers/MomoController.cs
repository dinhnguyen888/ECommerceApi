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
    private readonly IMomoService _momoService;
    private readonly ILogger<MomoController> _logger;
   

    public MomoController(IMomoService momoService , ILogger<MomoController> logger)
    {
        _momoService = momoService;
        _logger = logger;

    }

    [HttpGet("create-payment")]
    public async Task<IActionResult> CreatePayment([FromQuery] PaymentPostDto request)
    {
        try
        {
           var payUrl = await _momoService.CreatePaymentWithMomo(request);
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

            var handleStatus = await  _momoService.HandleMomoIpn(request);
            if(handleStatus =! true) return BadRequest(handleStatus); 
            return Ok();
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing Momo IPN.");
            return StatusCode(500, new { message = "An error occurred while processing the IPN request." });
        }
    }

}


