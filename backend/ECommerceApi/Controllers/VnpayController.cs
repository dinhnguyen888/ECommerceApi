using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _vnpayService;

        private readonly IConfiguration _configuration;

        public VnpayController(IVnpayService vnpayService, IConfiguration configuration)
        {
            _vnpayService = vnpayService;

            _configuration = configuration;
        }

        [HttpGet("CreatePaymentUrl")]
        public async Task<ActionResult<string>> CreatePaymentUrl( [FromQuery] PaymentPostDto payment)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // get IP address of equitment
                var paymentUrl = await _vnpayService.CreatePaymentInVnpay(payment, ipAddress);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("IpnAction")]
        public async Task<IActionResult> IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                        var paymentResult = await _vnpayService.handleIpnCallBack(Request.Query);

                        if(paymentResult == true)
                        return Redirect("/api/Vnpay/Callback"); // will change to frontend URL in the future
                        else return BadRequest("IPN Fail");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Can not find Payment info");
        }

        [HttpGet("Callback")]
        public async  Task<ActionResult<string>> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpayService.ProcessPaymentResult(Request.Query);
                    var result = "Bạn có thể tắt trang này được rồi!";
                    var resultDescription = $"{paymentResult.PaymentResponse.Description}, {result}.";

                    if (paymentResult.IsSuccess)
                    {
                        return Redirect(_configuration["URL:FrontendUrlPaymentCallback"]);
                    }

                    return BadRequest(resultDescription);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}