using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS;
using SmartE_Commerce_Business.DTOS.Common;
using SmartE_Commerce_Business.DTOS.Product;

namespace SmartE_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product : ControllerBase
    {
        private readonly IProductService prdcService;

        public Product(IProductService prdctService)
        {
            prdcService = prdctService;
        }

        //Admin Endpoints From API That brings and sending data 
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(prdcService.GetAllAsync());
        }

        [HttpGet("GetProductsPaged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50; // Limit max page size

            var result = await prdcService.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await prdcService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            await prdcService.AddAsync(dto);
            return Ok("Product Created");
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            await prdcService.UpdateAsync(dto);
            return Ok("Product Updated");
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await prdcService.DeleteAsync(id);
            return Ok("Deleted");
        }

    }
}
