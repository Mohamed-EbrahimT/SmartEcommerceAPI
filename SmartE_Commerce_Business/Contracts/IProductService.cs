using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartE_Commerce_Business.DTOS;
using SmartE_Commerce_Business.DTOS.Common;
using SmartE_Commerce_Business.DTOS.Product;

namespace SmartE_Commerce_Business.Contracts
{
    public interface IProductService
    {

        IEnumerable<ListProductsDto> GetAllAsync();
        Task<PagedResult<ListProductsDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<ProductDetailsDto?> GetByIdAsync(int id);
        Task<IEnumerable<ListProductsDto>> GetProductsByIdsAsync(List<int> ids);
        // OLD: Task<CreateProductDto> AddAsync(CreateProductDto dto);
        Task<ProductDetailsDto?> AddAsync(CreateProductDto dto);
        Task UpdateAsync(UpdateProductDto dto);
        Task DeleteAsync(int id);


        //Task InsertProduct(ProductInsertedDTO product);
        //void UpdateProduct(ProductInsertedDTO product);
    }
}
