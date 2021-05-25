using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class ProductDTO
    {
        public int id { get; set; }
        [StringLength(maximumLength:50, ErrorMessage = "Max length for title is 50")]
        public string title { get; set; }
        [StringLength(maximumLength: 100, ErrorMessage = "Max length for title is 100")]
        public string description { get; set; }
        [StringLength(maximumLength: 150, ErrorMessage = "Max length for title is 150")]
        public string imageUrl { get; set; }
        public double price { get; set; }
        public bool isFavorite { get; set; }
    }
}
