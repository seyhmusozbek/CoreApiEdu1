using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class Cart
    {
        public int id { get; set; }
        public Product productId { get; set; }
        public double quantity { get; set; }
    }
}
