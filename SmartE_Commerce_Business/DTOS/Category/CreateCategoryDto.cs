using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartE_Commerce_Business.DTOS.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
