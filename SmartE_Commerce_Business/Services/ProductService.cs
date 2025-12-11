using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FinalProj.Models;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS;
using SmartE_Commerce_Business.DTOS.Product;
using SmartE_Commerce_Data.Contracts;
using SmartE_Commerce_Data.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SmartE_Commerce_Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productRepon;
        private readonly IProductRepository productRepo;

        public ProductService(IRepository<Product> productsRepository, IProductRepository productRepos)
        {
            productRepon = productsRepository;
            productRepo = productRepos;
        }

        public IEnumerable<ListProductsDto> GetAllAsync()
        {
            var product = productRepo.GetAllAsync();
            var result = product.Select(p => new ListProductsDto
            {
                Id = p.ProductId,
                Name = p.ProductName,
                CategoryId = p.Category.CategoryId,
                ImageURL = p.Images
                .OrderBy(i => i.ImageId)
                .Select(i => i.ImageURL)
                .FirstOrDefault()
            });
            return result;
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        {
            var p = await productRepo.GetProductByIdAsync(id);
            if (p == null) return null;

            return new ProductDetailsDto
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.Price,
                CategoryId = p.CategoryId,
                ImagesURL = p.Images.Select(i => i.ImageURL).ToList(),
            };
        }

        public async Task AddAsync(CreateProductDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || dto.CategoryId < 1)
                return;
            var product = new Product
            {
                ProductName = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Stock = dto.Stock,
            };

            if (dto.Images != null)
            {
                foreach (var url in dto.Images)
                {
                    product.Images.Add(new Images
                    {
                        ImageURL = url,
                    });
                }
            }
            await productRepo.InsertAsync(product);
        }

        //Validated one
        public async Task UpdateAsync(UpdateProductDto dto)
        {
            var product = await productRepo.GetProductByIdAsync(dto.Id);

            if (product == null)
                return;

            // Update only provided values
            product.ProductName = dto.Name ?? dto.Name;

            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;

            if (dto.Stock.HasValue)
                product.Stock = dto.Stock.Value;

            product.CategoryId = dto.CategoryId;

            // If images are provided, update them (simple example)
            if (dto.Images != null && dto.Images.Any())
            {
                product.Images.Clear();

                foreach (var img in dto.Images)
                {
                    product.Images.Add(new Images
                    {
                        ImageURL = img,

                    });
                }
            }

            await productRepo.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await productRepo.DeleteProductAsync(id);
        }










        //public async Task InsertProduct(ProductInsertedDTO product)
        //{
        //    var entityprdct = new Product { ProductName = product.Name ,CategoryId = product.CategoryID , ProductId = product.Id};
        //    entityprdct.Images.Add(new Images { ImageURL = product.ImageURL });
        //    await productRepo.InsertAsync(entityprdct);
        //}

        //public void UpdateProduct(ProductUpdatedDTO product)
        //{
        //    var entityprdct = new Product { ProductName = product.Name, CategoryId = product.CategoryID, ProductId = product.Id };
        //    entityprdct.Images.Add(new Images { ImageURL = product.ImageURL });
        //    productRepo.Update(entityprdct);
        //}
    }
}
