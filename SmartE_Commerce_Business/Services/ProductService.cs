using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FinalProj.Models;
using SmartE_Commerce_Business.Contracts;
using SmartE_Commerce_Business.DTOS;
using SmartE_Commerce_Business.DTOS.Common;
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
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IEmbeddingService _embeddingService;

        // OLD CONSTRUCTOR (commented out - before Cloudinary integration)
        // public ProductService(IRepository<Product> productsRepository, IProductRepository productRepos)
        // {
        //     productRepon = productsRepository;
        //     productRepo = productRepos;
        // }

        public ProductService(
            IRepository<Product> productsRepository, 
            IProductRepository productRepos, 
            ICloudinaryService cloudinaryService,
            IEmbeddingService embeddingService)
        {
            productRepon = productsRepository;
            productRepo = productRepos;
            _cloudinaryService = cloudinaryService;
            _embeddingService = embeddingService;
        }

        public IEnumerable<ListProductsDto> GetAllAsync()
        {
            var product = productRepo.GetAllAsync();
            var result = product.Select(p => new ListProductsDto
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price=p.Price,
                CategoryId = p.Category?.CategoryId ?? 0,
                ImageURL = p.Images
                .OrderBy(i => i.ImageId)
                .Select(i => i.ImageURL)
                .FirstOrDefault()
            });
            return result;
        }

        public async Task<PagedResult<ListProductsDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var (products, totalCount) = await productRepo.GetPagedAsync(pageNumber, pageSize);
            
            var items = products.Select(p => new ListProductsDto
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.Price,
                CategoryId = p.Category?.CategoryId ?? 0,
                ImageURL = p.Images
                    .OrderBy(i => i.ImageId)
                    .Select(i => i.ImageURL)
                    .FirstOrDefault()
            });

            return new PagedResult<ListProductsDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        {
            var p = await productRepo.GetProductByIdAsync(id);
            if (p == null) return null;

            return new ProductDetailsDto
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                ImagesURL = p.Images.Select(i => i.ImageURL).ToList(),
            };
        }

        // OLD: public async Task<CreateProductDto> AddAsync(CreateProductDto dto)
        public async Task<ProductDetailsDto?> AddAsync(CreateProductDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || dto.CategoryId < 1)
                return null;
            var product = new Product
            {
                ProductName = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Stock = dto.Stock,
            };

            // OLD CODE (commented out - before Cloudinary integration)
            // if (dto.Images != null)
            // {
            //     foreach (var url in dto.Images)
            //     {
            //         product.Images.Add(new Images
            //         {
            //             ImageURL = url,
            //         });
            //     }
            // }

            // Upload images to Cloudinary and store the Cloudinary URLs
            var uploadedImageUrls = new List<string>();
            if (dto.Images != null && dto.Images.Any())
            {
                uploadedImageUrls = await _cloudinaryService.UploadImagesAsync(dto.Images);
                foreach (var cloudinaryUrl in uploadedImageUrls)
                {
                    product.Images.Add(new Images
                    {
                        ImageURL = cloudinaryUrl,
                    });
                }
            }

            await productRepo.InsertAsync(product);
            
            // Send to FastAPI for CLIP embeddings (fire and forget - don't block on failure)
            if (uploadedImageUrls.Any())
            {
                _ = _embeddingService.StoreProductEmbeddingsAsync(product.ProductId, uploadedImageUrls);
            }
            
            // Return the created product using existing ProductDetailsDto
            return new ProductDetailsDto
            {
                Id = product.ProductId,
                Name = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImagesURL = uploadedImageUrls
            };
        }

        //Validated one
        public async Task UpdateAsync(UpdateProductDto dto)
        {
            var product = await productRepo.GetProductByIdAsync(dto.Id);

            if (product == null)
                return;

            // Update only provided values
            product.ProductName = dto.Name ?? dto.Name;

            if (dto.Description != null)
                product.Description = dto.Description;

            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;

            if (dto.Stock.HasValue)
                product.Stock = dto.Stock.Value;

            product.CategoryId = dto.CategoryId;

            // OLD CODE (commented out - before Cloudinary integration)
            // If images are provided, update them (simple example)
            // if (dto.Images != null && dto.Images.Any())
            // {
            //     product.Images.Clear();
            //
            //     foreach (var img in dto.Images)
            //     {
            //         product.Images.Add(new Images
            //         {
            //             ImageURL = img,
            //         });
            //     }
            // }

            // Smart image update: only upload NEW local images, keep existing Cloudinary URLs
            var newlyUploadedUrls = new List<string>();
            var allImageUrls = new List<string>();
            
            if (dto.Images != null && dto.Images.Any())
            {
                // Remove existing images
                product.Images.Clear();

                foreach (var imagePath in dto.Images)
                {
                    // If already a Cloudinary URL, don't re-upload
                    if (imagePath.StartsWith("https://res.cloudinary.com") || 
                        imagePath.StartsWith("http://res.cloudinary.com"))
                    {
                        allImageUrls.Add(imagePath);
                        product.Images.Add(new Images 
                        { 
                            ImageURL = imagePath,
                            ProductId = product.ProductId  // Must set ProductId explicitly!
                        });
                    }
                    else
                    {
                        // It's a local file path, upload to Cloudinary
                        var cloudinaryUrl = await _cloudinaryService.UploadImageAsync(imagePath);
                        if (!string.IsNullOrEmpty(cloudinaryUrl))
                        {
                            allImageUrls.Add(cloudinaryUrl);
                            newlyUploadedUrls.Add(cloudinaryUrl);
                            product.Images.Add(new Images 
                            { 
                                ImageURL = cloudinaryUrl,
                                ProductId = product.ProductId  // Must set ProductId explicitly!
                            });
                        }
                    }
                }
            }

            await productRepo.UpdateAsync(product);

            // Only send NEW images to FastAPI for embeddings (not existing ones already in Qdrant)
            if (newlyUploadedUrls.Any())
            {
                _ = _embeddingService.StoreProductEmbeddingsAsync(product.ProductId, newlyUploadedUrls);
            }
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
