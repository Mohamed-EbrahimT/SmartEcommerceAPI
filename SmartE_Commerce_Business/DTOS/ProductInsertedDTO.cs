using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;
using SmartE_Commerce_Data.Models;

namespace SmartE_Commerce_Business.DTOS
{
    public class ProductInsertedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
    }
}
