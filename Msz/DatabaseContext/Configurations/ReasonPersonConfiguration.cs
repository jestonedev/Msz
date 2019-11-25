using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.DatabaseContext.Configurations
{

    public class ReasonPersonConfiguration : IEntityTypeConfiguration<ReasonPerson>
    {
        public void Configure(EntityTypeBuilder<ReasonPerson> builder)
        {
            builder.HasOne(p => p.Gender).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
