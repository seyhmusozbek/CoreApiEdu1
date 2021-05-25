using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class Product
    {
        public int id { get; set; }
        [MaxLength(50)]
        public string title { get; set; }
        [MaxLength(100)]
        public string description { get; set; }
        [MaxLength(150)]
        public string imageUrl { get; set; }
        public double price { get; set; }
        public bool isFavorite { get; set; }
    }
}
