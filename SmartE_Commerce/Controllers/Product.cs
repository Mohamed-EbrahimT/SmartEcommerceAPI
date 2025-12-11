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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(prdcService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await prdcService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            await prdcService.AddAsync(dto);
            return Ok("Product Created");
        }

    }
}
