using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;

namespace SmartE_Commerce_Data.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetAllAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task DeleteCategoryAsync(int id);
    }
}
