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
        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> ids);

        //Task AddAsync(Product product);
        //Task UpdateAsync(Product product);
        Task DeleteProductAsync(int id);

    }
}
