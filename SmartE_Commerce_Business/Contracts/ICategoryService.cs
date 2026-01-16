using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartE_Commerce_Business.DTOS.Category;

namespace SmartE_Commerce_Business.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<ListCategoriesDto> GetAllAsync();
        Task<CategoryDetailsDto?> GetByIdAsync(int id);
        Task<CreateCategoryDto> AddAsync(CreateCategoryDto dto);
        Task UpdateAsync(UpdateCategoryDto dto);
        Task DeleteAsync(int id);
    }
}
