using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreApiEdu1.Entities;

namespace CoreApiEdu1.Models
{
    public class CartDTO
    {
        public int id { get; set; }
        [Required]
        public Product productId { get; set; }
        [Required]
        public double quantity { get; set; }
    }
}
