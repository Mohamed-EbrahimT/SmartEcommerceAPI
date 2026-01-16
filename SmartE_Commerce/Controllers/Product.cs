using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS;
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
