using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreApiEdu1.Entities
{
    public class BarcodeContext:IdentityDbContext<AppUser>
    {
        public BarcodeContext(DbContextOptions ops):base(ops)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> CartItems { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MStop> MStops { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
