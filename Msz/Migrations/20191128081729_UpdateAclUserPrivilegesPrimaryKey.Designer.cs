﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Msz.DatabaseContext;

namespace Msz.Migrations
{
    [DbContext(typeof(MszDbContext))]
    [Migration("20191128081729_UpdateAclUserPrivilegesPrimaryKey")]
    partial class UpdateAclUserPrivilegesPrimaryKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Msz.Models.AclPrivilege", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AclPrivilege");

                    b.HasData(
                        new { Id = 2, Name = "Изменение" }
                    );
                });

            modelBuilder.Entity("Msz.Models.AclUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AclUsers");
                });

            modelBuilder.Entity("Msz.Models.AclUserPrivilege", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PrivilegeId");

                    b.Property<int>("MszId");

                    b.HasKey("UserId", "PrivilegeId", "MszId");

                    b.HasIndex("MszId");

                    b.HasIndex("PrivilegeId");

                    b.ToTable("AclUserPrivilege");
                });

            modelBuilder.Entity("Msz.Models.AssigmentForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AssigmentForms");

                    b.HasData(
                        new { Id = 1, Name = "01 Денежная" },
                        new { Id = 3, Name = "03 Льготы" }
                    );
                });

            modelBuilder.Entity("Msz.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Guid")
                        .IsRequired();

                    b.Property<int>("MszId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MszId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Msz.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Genders");

                    b.HasData(
                        new { Id = 1, Name = "Мужской" },
                        new { Id = 2, Name = "Женский" }
                    );
                });

            modelBuilder.Entity("Msz.Models.Msz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<string>("Guid")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("NextRevisionId");

                    b.Property<string>("PreviousGuid");

                    b.Property<int?>("PreviousRevisionId");

                    b.HasKey("Id");

                    b.ToTable("Mszs");
                });

            modelBuilder.Entity("Msz.Models.ReasonPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate");

                    b.Property<int>("GenderId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Patronymic");

                    b.Property<int>("ReceiverId");

                    b.Property<string>("Snils")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("ReasonPerson");
                });

            modelBuilder.Entity("Msz.Models.Receiver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<decimal>("Amount");

                    b.Property<int>("AssigmentFormId");

                    b.Property<DateTime>("BirthDate");

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<DateTime>("DecisionDate");

                    b.Property<string>("DecisionNumber");

                    b.Property<DateTime?>("EndDate");

                    b.Property<decimal?>("EquivalentAmount");

                    b.Property<int>("GenderId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("MszId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("NextRevisionId");

                    b.Property<string>("Patronymic");

                    b.Property<string>("Phone");

                    b.Property<int?>("PrevRevisionId");

                    b.Property<string>("Snils")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.Property<string>("Uuid")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AssigmentFormId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("GenderId");

                    b.HasIndex("MszId");

                    b.ToTable("Receivers");
                });

            modelBuilder.Entity("Msz.Models.AclUserPrivilege", b =>
                {
                    b.HasOne("Msz.Models.Msz", "Msz")
                        .WithMany()
                        .HasForeignKey("MszId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Msz.Models.AclPrivilege", "Privilege")
                        .WithMany("Users")
                        .HasForeignKey("PrivilegeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msz.Models.AclUser", "User")
                        .WithMany("Privileges")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Msz.Models.Category", b =>
                {
                    b.HasOne("Msz.Models.Msz", "Msz")
                        .WithMany("Categories")
                        .HasForeignKey("MszId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Msz.Models.ReasonPerson", b =>
                {
                    b.HasOne("Msz.Models.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msz.Models.Receiver", "Receiver")
                        .WithMany("ReasonPersons")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Msz.Models.Receiver", b =>
                {
                    b.HasOne("Msz.Models.AssigmentForm", "AssigmentForm")
                        .WithMany()
                        .HasForeignKey("AssigmentFormId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msz.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msz.Models.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msz.Models.Msz", "Msz")
                        .WithMany()
                        .HasForeignKey("MszId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
