using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(string userId)
        {
            var cartItems = await _cartService.GetCartByUserId(userId);
            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Cart cart)
        {
            var createdCart = await _cartService.AddToCart(cart);
            return CreatedAtAction(nameof(GetCartByUserId), new { userId = cart.UserId }, createdCart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(string id)
        {
            var isRemoved = await _cartService.RemoveFromCart(id);
            if (!isRemoved)
            {
                return NotFound(new { message = "Cart item not found." });
            }
            return NoContent();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var isCleared = await _cartService.ClearCart(userId);
            if (!isCleared)
            {
                return NotFound(new { message = "No cart items found for this user." });
            }
            return NoContent();
        }
    }
}
