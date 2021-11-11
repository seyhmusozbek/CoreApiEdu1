using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiEdu1.Configurations.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreApiEdu1.Entities
{
    public class BarcodeContext : IdentityDbContext<AppUser>
    {
        public BarcodeContext(DbContextOptions ops) : base(ops)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> CartItems { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MStop> MStops { get; set; }
        public DbSet<ChosenOrder> ChosenOrders { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<StockReserve> StockReserves { get; set; }
        public DbSet<WHTransfer> WHTransfers { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<CountDetail> CountDetails { get; set; }
        public DbSet<CountMaster> CountMaster { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfiguration(new RoleConfiguration());
        }

    }
}
