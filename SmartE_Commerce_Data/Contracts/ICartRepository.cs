using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;

namespace SmartE_Commerce_Data.Contracts
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartWithItemsAsync(int cartId);
        Task<CartItem?> CheckItemAsync(int cartId, int productId);
        Task AddItemToCartAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(int cartItemId);
        Task ClearCartAsync(int cartId);
    }
}
