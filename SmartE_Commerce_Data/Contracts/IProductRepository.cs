using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;

namespace SmartE_Commerce_Data.Contracts
{
    public interface IProductRepository:IRepository<Product>
    {
        IEnumerable<Product> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

    }
}
