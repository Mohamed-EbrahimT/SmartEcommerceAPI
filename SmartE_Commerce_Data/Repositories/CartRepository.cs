using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Data;
using FinalProj.Models;
using Microsoft.EntityFrameworkCore;
using SmartE_Commerce_Data.Contracts;

namespace SmartE_Commerce_Data.Repositories
{
    internal class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ECContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetCartWithItemsAsync(int cartId)
        {
            return await context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task<CartItem?> CheckItemAsync(int cartId, int productId)
        {
            return await context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task AddItemToCartAsync(CartItem item)
        {
            await context.CartItems.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem item)
        {
            context.CartItems.Update(item);
            await context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var item = await context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                context.CartItems.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int cartId)
        {
            var items = await context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
            
            context.CartItems.RemoveRange(items);
            await context.SaveChangesAsync();
        }
    }
}
