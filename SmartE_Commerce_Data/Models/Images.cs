using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProj.Models;

namespace SmartE_Commerce_Data.Models
{
    public partial class Images
    {
        [Key]
        public int ImageId { get; set; }

        public string ImageURL { get; set; }
        public string? ImageTitle { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
