using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.DatabaseContext.Configurations
{

    public class ReceiverConfiguration : IEntityTypeConfiguration<Receiver>
    {
        public void Configure(EntityTypeBuilder<Receiver> builder)
        {
            builder.HasOne(p => p.Msz).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Category).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.AssigmentForm).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Gender).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
