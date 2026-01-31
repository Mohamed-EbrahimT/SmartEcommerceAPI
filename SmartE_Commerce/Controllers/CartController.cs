using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS.Cart;
using System.Security.Claims;

namespace SmartE_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires JWT authentication for all endpoints
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        // Extract user ID from JWT claims
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("userId")?.Value;
            
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetCurrentUserId();
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null)
                return Ok(new { message = "Cart is empty", items = new List<object>() });

            return Ok(cart);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            try
            {
                int userId = GetCurrentUserId();
                var cart = await _cartService.AddToCartAsync(userId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCartItem")]
        public async Task<IActionResult> UpdateCartItem(UpdateCartItemDto dto)
        {
            int userId = GetCurrentUserId();
            await _cartService.UpdateCartItemAsync(userId, dto);
            return Ok("Cart item updated");
        }

        [HttpDelete("RemoveFromCart/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            int userId = GetCurrentUserId();
            await _cartService.RemoveFromCartAsync(userId, cartItemId);
            return Ok("Item removed from cart");
        }

        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            int userId = GetCurrentUserId();
            await _cartService.ClearCartAsync(userId);
            return Ok("Cart cleared");
        }
    }
}
