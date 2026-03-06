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
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ECContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetAllAsync()
        {
            return context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsEnumerable();
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await context.Products.CountAsync();
            
            var items = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .OrderBy(p => p.ProductId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products
                .Include(p => p.Category)
                .Include (p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId==id);    
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            return await context.Products
                .Where(p => ids.Contains(p.ProductId))
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }

        // انت كدا كدا هتجيبهم من الريبو عن طريق الانهرتنس
        //public async Task AddAsync(Product product)
        //{
        //    await context.Products.AddAsync(product);
        //    await context.SaveChangesAsync();
        //}
        //async Task IProductRepository.UpdateAsync(Product product)
        //{
        //    context.Products.Update(product);
        //    await context.SaveChangesAsync();
        //}

        public async Task DeleteProductAsync(int id)
        {
            var p = await GetByIdAsync(id);
            if (p != null)
            {
                context.Products.Remove(p);
                await context.SaveChangesAsync();
            }
            else
                return;


        }







    }
}
