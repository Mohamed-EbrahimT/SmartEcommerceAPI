using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartE_Commerce_Business.DTOS;

namespace SmartE_Commerce_Business.Contracts
{
    public interface IProductService
    {
        Task InsertProduct(ProductInsertedDTO product);
    }
}
