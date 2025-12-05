using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductInsertedDTO dto)
        {
            await prdcService.InsertProduct(dto);
            return Ok();
        }


    }
}
