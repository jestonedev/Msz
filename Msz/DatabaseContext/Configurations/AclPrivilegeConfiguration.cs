using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.DatabaseContext.Configurations
{

    public class AclPrivilegeConfiguration : IEntityTypeConfiguration<AclPrivilege>
    {
        public void Configure(EntityTypeBuilder<AclPrivilege> builder)
        {
            builder.HasMany(p => p.Users).WithOne(p => p.Privilege)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
