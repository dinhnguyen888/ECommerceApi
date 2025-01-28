using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnpayPaymentController : ControllerBase
    {
        private readonly VnpayService _vnpayService;
        private readonly IPaymentService _paymentService;   

        public VnpayPaymentController(VnpayService vnpayService, IPaymentService paymentService)
        {
            _vnpayService = vnpayService;
            _paymentService = paymentService;
        }

        [HttpGet("CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl([FromQuery]long moneyToPay,[FromBody] PaymentPostDto payment )
        {
            try
            {
                var description = _paymentService.ConvertPaymentInfoToDescriptionInVnpay(payment);
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch
                var paymentUrl = _vnpayService.CreatePaymentUrl(moneyToPay, description, ipAddress);
                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("IpnAction")]
        public IActionResult IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpayService.ProcessPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                      
                        //get a description in paymentResult
                        string description = paymentResult.PaymentResponse.Description;

                        var addToPayment =_paymentService.SplitDescriptionAndCreatePayment(description);

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
        public ActionResult<string> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpayService.ProcessPaymentResult(Request.Query);
                    var resultDescription = $"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.";

                    if (paymentResult.IsSuccess)
                    {
                        return Ok(resultDescription);
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
