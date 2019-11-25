﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Msz.Models;
using Msz.DatabaseContext.Configurations;

namespace Msz.DatabaseContext
{
    public class MszDbContext : DbContext
    {
        public DbSet<Models.Msz> Mszs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<AssigmentForm> AssigmentForms { get; set; }
        public DbSet<Receiver> Receivers { get; set; }

        public MszDbContext()
        {
        }

        public MszDbContext(DbContextOptions<MszDbContext> options, IConfiguration config)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiverConfiguration());
            modelBuilder.ApplyConfiguration(new ReasonPersonConfiguration());
            modelBuilder.Entity<Gender>().HasData(new Gender[] {
                new Gender { Id = 1, Name = "Мужской" },
                new Gender { Id = 2, Name = "Женский" }});
            modelBuilder.Entity<AssigmentForm>().HasData(new AssigmentForm[] {
                new AssigmentForm { Id = 1, Name = "01 Денежная" },
                new AssigmentForm { Id = 3, Name = "03 Льготы" }});
        }
    }
}
