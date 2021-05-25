using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreApiEdu1.Entities
{
    public class BarcodeContext:DbContext
    {
        public BarcodeContext(DbContextOptions ops):base(ops)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> CartItems { get; set; }
    }
}
