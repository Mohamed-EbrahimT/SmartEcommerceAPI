using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS;
using SmartE_Commerce_Data.Contracts;
using SmartE_Commerce_Data.Models;

namespace SmartE_Commerce_Business.Services
{
    public class ProductService:IProductService
    {
        private readonly IRepository<Product> productRepo;

        public ProductService(IRepository<Product> productsRepository)
        {
            productRepo = productsRepository;
        }

        public async Task InsertProduct(ProductInsertedDTO product)
        {
            var entityprdct = new Product { ProductName = product.Name ,CategoryId = product.CategoryID , ProductId = product.Id};
            entityprdct.Images.Add(new Images { ImageURL = product.ImageURL });
            await productRepo.InsertAsync(entityprdct);
        }
    }
}
