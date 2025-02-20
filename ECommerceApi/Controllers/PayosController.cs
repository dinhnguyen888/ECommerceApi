
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json.Linq;
using Net.payOS.Utils;
using ECommerceApi.Dtos;
using static System.Net.WebRequestMethods;

namespace ECommerceApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PayosController : ControllerBase
    {
        private readonly PayosService _payOS;
        public PayosController(PayosService payOS)
        {
            _payOS = payOS;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink([FromQuery] PaymentPostDto body)
        {
            try
            {
                var createPayosPayment = await _payOS.CreatePaymentLink(body);

                return Ok(createPayosPayment);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("ipn")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookType? webhookBody)
        {
            try
            {
                var result = await _payOS.ReceiveWebhook(webhookBody);
                if (webhookBody == null) return Ok();
                return Ok();
            }
            catch (ArgumentNullException arEx)
            {
                // accept argumentNull because payos will send webhook with sample input,
                // paymentId not exist with sample input
                return Ok(); 
            }
            catch (HttpRequestException httpEx)
            {
                // Log lỗi cụ thể khi gọi HTTP thất bại
                Console.WriteLine($"HttpRequestException: {httpEx}");
                return BadRequest(httpEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return BadRequest(new { error = ex.Message });
            }

        }


    }

}