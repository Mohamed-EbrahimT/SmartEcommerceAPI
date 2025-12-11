using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartE_Commerce_Business.DTOS.Product
{
    public class ListProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }

        private string _ImageURL;
        public string ImageURL 
        {
            get=>_ImageURL;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Image Not Found");
                _ImageURL = value;
            } 
        }
    }
}
