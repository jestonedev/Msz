using Microsoft.EntityFrameworkCore;
using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.DatabaseContext
{
    public interface IMszDbContext
    {
        DbSet<Models.Msz> Mszs { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Gender> Genders { get; set; }
        DbSet<AssigmentForm> AssigmentForms { get; set; }
        DbSet<Receiver> Receivers { get; set; }
        DbSet<AclUser> AclUsers { get; set; }

        int SaveChanges();
    }
}
