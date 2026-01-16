using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS.Category;
using SmartE_Commerce_Data.Contracts;

namespace SmartE_Commerce_Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> categoryRepo;
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(IRepository<Category> categoryRepo, ICategoryRepository categoryRepository)
        {
            this.categoryRepo = categoryRepo;
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<ListCategoriesDto> GetAllAsync()
        {
            var categories = categoryRepository.GetAllAsync();
            var result = categories.Select(c => new ListCategoriesDto
            {
                Id = c.CategoryId,
                Name = c.CategoryName,
                Description = c.Description,
                ProductCount = c.Products?.Count ?? 0
            });
            return result;
        }

        public async Task<CategoryDetailsDto?> GetByIdAsync(int id)
        {
            var c = await categoryRepository.GetCategoryByIdAsync(id);
            if (c == null) return null;

            return new CategoryDetailsDto
            {
                Id = c.CategoryId,
                Name = c.CategoryName,
                Description = c.Description,
                Products = c.Products?.Select(p => new CategoryProductDto
                {
                    Id = p.ProductId,
                    Name = p.ProductName,
                    Price = p.Price,
                    ImageURL = p.Images?.OrderBy(i => i.ImageId).Select(i => i.ImageURL).FirstOrDefault()
                }).ToList() ?? new List<CategoryProductDto>()
            };
        }

        public async Task<CreateCategoryDto> AddAsync(CreateCategoryDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name))
                return null;

            var category = new Category
            {
                CategoryName = dto.Name,
                Description = dto.Description
            };

            await categoryRepository.InsertAsync(category);
            return dto;
        }

        public async Task UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(dto.Id);

            if (category == null)
                return;

            if (!string.IsNullOrEmpty(dto.Name))
                category.CategoryName = dto.Name;

            if (dto.Description != null)
                category.Description = dto.Description;

            await categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            await categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
