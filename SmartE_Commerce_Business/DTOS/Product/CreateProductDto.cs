using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartE_Commerce_Data.Models;

namespace SmartE_Commerce_Business.DTOS.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string? Description { get; set; } = "";
        public decimal? Price { get; set; } = 0;
        public int? Stock { get; set; } = 0;
        public int CategoryId { get; set; }
        public List<string> Images { get; set; } = null;

    }
}
