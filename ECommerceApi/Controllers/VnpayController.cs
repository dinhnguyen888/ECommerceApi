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
        private readonly VnpayService _vnpayService;
        private readonly IPaymentService _paymentService;

        public VnpayController(VnpayService vnpayService, IPaymentService paymentService)
        {
            _vnpayService = vnpayService;
            _paymentService = paymentService;
        }

        [HttpGet("CreatePaymentUrl")]
        public async Task<ActionResult<string>> CreatePaymentUrl( [FromQuery] PaymentPostDto payment)
        {
            try
            {
                
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // get IP address of equitment
                var moneyToPay = payment.ProductPrice;
                var paymentId = await _paymentService.CreatePaymentAsync(payment);
                var description = paymentId; //use description to store paymentId
                var paymentUrl = _vnpayService.CreatePaymentUrl(moneyToPay, description, ipAddress);

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
                    var paymentResult = _vnpayService.ProcessPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                        //var description = paymentResult.Description;


                        //var payment = await _vnpayService.ChangePaymentStatusAndGetPaymentInfo(true, description);
                        //await _vnpayService.SendEmailUsingPaymentInfo(payment);

                      

                        return Ok();
                    }


                    return BadRequest("Thanh toán thất bại");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        [HttpGet("Callback")]
        public async  Task<ActionResult<string>> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpayService.ProcessPaymentResult(Request.Query);
                    var resultDescription = $"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.";

                    if (paymentResult.IsSuccess)
                    {
                        var description = paymentResult.Description;

                        var paymentId = Convert.ToInt64(description);

                        var payment = await _vnpayService.ChangePaymentStatusAndGetPaymentInfo(true, paymentId);
                        await _vnpayService.SendEmailUsingPaymentInfo(payment);

                        return Ok(paymentResult);
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