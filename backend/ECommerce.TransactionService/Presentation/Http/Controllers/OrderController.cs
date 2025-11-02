using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Interfaces;

namespace ECommerce.TransactionService.Presentation.Http.Controllers
{
    // Controller xu ly cac API lien quan den Order
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/order
        // Tao don hang moi
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

        // GET: api/order/{id}
        // Lay don hang theo ID
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

        // GET: api/order/user/{userId}
        // Lay tat ca don hang cua user
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

        // GET: api/order
        // Lay tat ca don hang (chi admin)
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
    }
}
