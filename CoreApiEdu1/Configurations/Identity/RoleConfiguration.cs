using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Configurations.Identity
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "ExtUser",
                    NormalizedName = "ExtUser",
                },
                new IdentityRole
                {
                    Name = "LamUser",
                    NormalizedName = "LAMUSER",
                },
                new IdentityRole
                {
                    Name = "FensUser",
                    NormalizedName = "FENSUSER",
                },
                new IdentityRole
                {
                    Name = "Stocker",
                    NormalizedName = "STOCKER",
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "Observer",
                    NormalizedName = "OBSERVER"
                },
                new IdentityRole
                {
                    Name = "Planner",
                    NormalizedName = "PLANNER"
                }
                );
        }
    }
}
