using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartE_Commerce_Business.DTOS.Cart;

namespace SmartE_Commerce_Business.Contracts
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int userId);
        Task<CartDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task UpdateCartItemAsync(int userId, UpdateCartItemDto dto);
        Task RemoveFromCartAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
    }
}
