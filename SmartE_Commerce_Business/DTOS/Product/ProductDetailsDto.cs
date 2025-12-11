using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartE_Commerce_Business.DTOS.Product
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; } = 0;
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public List<string> ImagesURL { get; set; }
        
    }
}
