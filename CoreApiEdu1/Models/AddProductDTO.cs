using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class AddProductDTO
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Max length for title is 100")]
        public string title { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Max length for description is 100")]
        public string description { get; set; }
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "Max length for image url is 150")]
        public string imageUrl { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public bool isFavorite { get; set; }
    }
    public class UpdateProductDTO:AddProductDTO
    {

    }
}
