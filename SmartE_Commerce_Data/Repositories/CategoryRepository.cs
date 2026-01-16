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
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ECContext context) : base(context)
        {
        }

        public IEnumerable<Category> GetAllAsync()
        {
            return context.Categories
                .Include(c => c.Products)
                .AsEnumerable();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
                
            if (category != null)
            {
                // Set CategoryId to null on all related products - Before deleting the category to prevent the fk error
                foreach (var product in category.Products)
                {
                    product.CategoryId = null;
                }
                
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }
    }
}
