using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.DatabaseContext.Configurations
{

    public class AclUserConfiguration : IEntityTypeConfiguration<AclUser>
    {
        public void Configure(EntityTypeBuilder<AclUser> builder)
        {
            builder.HasMany(p => p.Privileges).WithOne(p => p.User)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
