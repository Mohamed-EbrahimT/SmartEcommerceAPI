using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS.Category;

namespace SmartE_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Category : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public Category(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(categoryService.GetAllAsync());
        }

        [HttpGet("GetCategory/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var result = await categoryService.AddAsync(dto);
            if (result == null) return BadRequest("Invalid category data");
            return Ok("Category Created");
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            await categoryService.UpdateAsync(dto);
            return Ok("Category Updated");
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await categoryService.DeleteAsync(id);
            return Ok("Deleted");
        }
    }
}
