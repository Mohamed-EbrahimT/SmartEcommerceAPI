using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS.Cart;
using SmartE_Commerce_Data.Contracts;

namespace SmartE_Commerce_Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public CartService(ICartRepository cartRepo, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<CartDto?> GetCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return null;

            return MapToCartDto(cart);
        }

        public async Task<CartDto> AddToCartAsync(int userId, AddToCartDto dto)
        {
            // Get or create cart for user
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Total = 0,
                    ItemTotal = 0
                };
                await _cartRepo.InsertAsync(cart);
                cart = await _cartRepo.GetCartByUserIdAsync(userId);
            }

            // Get product to get price
            var product = await _productRepo.GetProductByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            // Check if item already in cart
            var existingItem = await _cartRepo.CheckItemAsync(cart.CartId, dto.ProductId);
            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity = (existingItem.Quantity ?? 0) + dto.Quantity;
                existingItem.Price = product.Price;
                await _cartRepo.UpdateCartItemAsync(existingItem);
            }
            else
            {
                // Add new item
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    Price = product.Price
                };
                await _cartRepo.AddItemToCartAsync(cartItem);
            }

            // Recalculate cart totals
            await RecalculateCartTotals(cart.CartId);

            // Return updated cart
            var updatedCart = await _cartRepo.GetCartWithItemsAsync(cart.CartId);
            return MapToCartDto(updatedCart!);
        }

        public async Task UpdateCartItemAsync(int userId, UpdateCartItemDto dto)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return;

            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == dto.CartItemId);
            if (item == null) return;

            if (dto.Quantity <= 0)
            {
                await _cartRepo.RemoveCartItemAsync(dto.CartItemId);
            }
            else
            {
                item.Quantity = dto.Quantity;
                await _cartRepo.UpdateCartItemAsync(item);
            }

            await RecalculateCartTotals(cart.CartId);
        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return;

            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (item == null) return;

            await _cartRepo.RemoveCartItemAsync(cartItemId);
            await RecalculateCartTotals(cart.CartId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return;

            await _cartRepo.ClearCartAsync(cart.CartId);
            
            cart.Total = 0;
            cart.ItemTotal = 0;
            await _cartRepo.UpdateAsync(cart);
        }

        private async Task RecalculateCartTotals(int cartId)
        {
            var cart = await _cartRepo.GetCartWithItemsAsync(cartId);
            if (cart == null) return;

            cart.Total = cart.CartItems.Sum(ci => (ci.Price ?? 0) * (ci.Quantity ?? 0));
            cart.ItemTotal = cart.CartItems.Sum(ci => ci.Quantity ?? 0);
            await _cartRepo.UpdateAsync(cart);
        }

        private CartDto MapToCartDto(Cart cart)
        {
            return new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId ?? 0,
                Total = cart.Total ?? 0,
                ItemCount = cart.ItemTotal ?? 0,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId ?? 0,
                    ProductName = ci.Product?.ProductName ?? "",
                    Price = ci.Price ?? 0,
                    Quantity = ci.Quantity ?? 0,
                    Subtotal = (ci.Price ?? 0) * (ci.Quantity ?? 0),
                    ImageURL = ci.Product?.Images?.OrderBy(i => i.ImageId).Select(i => i.ImageURL).FirstOrDefault()
                }).ToList()
            };
        }
    }
}
