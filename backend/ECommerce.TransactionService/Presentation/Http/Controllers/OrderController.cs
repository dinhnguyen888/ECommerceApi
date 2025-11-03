using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Application.Events;

namespace ECommerce.TransactionService.Presentation.Http.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IBuyNowService _buyNowService;
        private readonly IRequestReplyService _requestReplyService;

        public OrderController(
            IOrderService orderService,
            IBuyNowService buyNowService,
            IRequestReplyService requestReplyService)
        {
            _orderService = orderService;
            _buyNowService = buyNowService;
            _requestReplyService = requestReplyService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderGetDto>> CreateOrder([FromBody] OrderCreateDto dto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(dto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderGetDto>> GetOrderById(string id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new { message = $"Khong tim thay don hang voi ID: {id}" });

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<List<OrderGetDto>>> GetOrdersByUserId(string userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderGetDto>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderGetDto>> UpdateOrder(string id, [FromBody] OrderUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { message = "ID trong URL va body khong khop" });

                var order = await _orderService.UpdateOrderAsync(dto);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOrder(string id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }

        [HttpPost("buy-now")]
        [Authorize]
        public async Task<ActionResult<BuyNowResponseDto>> BuyNow([FromBody] BuyNowDto dto)
        {
            try
            {
                // Step 1: Validate User - Request-Reply pattern
                var userValidationRequest = new UserValidationRequestEvent
                {
                    RequestId = Guid.NewGuid().ToString(),
                    UserId = dto.UserId
                };

                UserValidationResponseEvent userValidationResponse;
                try
                {
                    userValidationResponse = await _requestReplyService.RequestAsync<
                        UserValidationRequestEvent,
                        UserValidationResponseEvent>(
                        "user.validation.request",
                        userValidationRequest,
                        TimeSpan.FromSeconds(10)); // Timeout 10 giay
                }
                catch (TimeoutException ex)
                {
                    return StatusCode(408, new { message = "Khong nhan duoc phan hoi tu AuthService", error = ex.Message });
                }

                if (!userValidationResponse.IsValid)
                {
                    return BadRequest(new { message = $"User validation failed: {userValidationResponse.ErrorMessage ?? "User khong hop le"}" });
                }

                // Step 2: Validate Product - Request-Reply pattern
                var productValidationRequest = new ProductValidationRequestEvent
                {
                    RequestId = Guid.NewGuid().ToString(),
                    ProductIds = new List<string> { dto.ProductId }
                };

                ProductValidationResponseEvent productValidationResponse;
                try
                {
                    productValidationResponse = await _requestReplyService.RequestAsync<
                        ProductValidationRequestEvent,
                        ProductValidationResponseEvent>(
                        "product.validation.request",
                        productValidationRequest,
                        TimeSpan.FromSeconds(10)); // Timeout 10 giay
                }
                catch (TimeoutException ex)
                {
                    return StatusCode(408, new { message = "Khong nhan duoc phan hoi tu CommerceService", error = ex.Message });
                }

                if (productValidationResponse.ProductValidationResults == null ||
                    !productValidationResponse.ProductValidationResults.ContainsKey(dto.ProductId) ||
                    !productValidationResponse.ProductValidationResults[dto.ProductId])
                {
                    return BadRequest(new { message = $"Product validation failed: {productValidationResponse.ErrorMessage ?? "Product khong hop le"}" });
                }

                // Step 3: Neu validation thanh cong, tao order, payment record va VNPay payment URL
                // BuyNowService se:
                // - Tao order voi status Pending, ExpiresAt = +30 phut
                // - Tao payment record voi status Pending
                // - Tao VNPay payment URL de nguoi dung thanh toan
                // - Tra ve BuyNowResponseDto bao gom PaymentUrl
                var result = await _buyNowService.BuyNowAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Loi he thong", error = ex.Message });
            }
        }
    }
}
