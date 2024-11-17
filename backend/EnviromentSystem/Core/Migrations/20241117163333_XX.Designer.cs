﻿// <auto-generated />
using System;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241117163333_XX")]
    partial class XX
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Data.Entity.Building", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("E_MeterCode")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("G_MeterCode")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Buildings", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entity.Electric", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("FinalMeterValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InitialMeterValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("KWHValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("SchoolInfoId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Usage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("SchoolInfoId");

                    b.ToTable("Electrics", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entity.NaturalGas", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("FinalMeterValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InitialMeterValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("SM3Value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Usage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.ToTable("NaturalGasUsages", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entity.Paper", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Consumption")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SchoolInfoId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SchoolInfoId");

                    b.ToTable("Papers", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entity.SchoolInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<int>("NumberOfPeople")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SchoolInfos", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entity.User.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("eb9ecb38-c6e9-42d9-9295-ce2c307b0181"),
                            Name = "SUPERADMIN",
                            NormalizedName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = new Guid("4418aba5-5187-4c26-b0bc-c41ced161e03"),
                            Name = "ADMIN",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("166a4262-32fa-4368-898d-d41e2cb08fb6"),
                            Name = "USER",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Core.Data.Entity.User.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("82140c4f-784b-4af9-bb73-f52f019f7beb"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4a910417-eb6c-4ef9-9ff9-dee07497ac07",
                            Email = "superadmin@example.com",
                            EmailConfirmed = false,
                            IsConfirmed = true,
                            LockoutEnabled = false,
                            Name = "SUPERADMIN",
                            NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                            NormalizedUserName = "SUPERADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEIiukV3m/Zw0bAI6oBQekZjCe30a4ma/vxkaCImBuiByJGNbaMhgMjElqmUblcJ0+Q==",
                            PhoneNumberConfirmed = false,
                            Surname = "USER",
                            TwoFactorEnabled = false,
                            UserName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = new Guid("83c85d03-cb46-4ebb-b52f-315eaad91ede"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "6c57829f-22fd-4702-a3f2-87a5f4b04ab7",
                            Email = "admin@example.com",
                            EmailConfirmed = false,
                            IsConfirmed = true,
                            LockoutEnabled = false,
                            Name = "ADMIN",
                            NormalizedEmail = "ADMIN@EXAMPLE.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEFMXqcHmPsKRLpWFLCf8N/JtduPHR27eb/50TxAj3DKk2U2560vyE9+PuLKZysuPTQ==",
                            PhoneNumberConfirmed = false,
                            Surname = "USER",
                            TwoFactorEnabled = false,
                            UserName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("167326c1-11c3-494c-aa9b-9e0e42ee4837"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "473eb3ce-981a-4658-afae-36127e3c306f",
                            Email = "user@example.com",
                            EmailConfirmed = false,
                            IsConfirmed = true,
                            LockoutEnabled = false,
                            Name = "USER",
                            NormalizedEmail = "USER@EXAMPLE.COM",
                            NormalizedUserName = "USER",
                            PasswordHash = "AQAAAAIAAYagAAAAELDO86DJGKCQVJVT4EecFwA3Za8B3fO5YGfQRFsOl6aMLfOh93ThDFUujEMuy+gQWg==",
                            PhoneNumberConfirmed = false,
                            Surname = "USER",
                            TwoFactorEnabled = false,
                            UserName = "USER"
                        });
                });

            modelBuilder.Entity("Core.Data.Entity.Water", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Consumption")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SchoolInfoId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SchoolInfoId");

                    b.ToTable("Waters", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("82140c4f-784b-4af9-bb73-f52f019f7beb"),
                            RoleId = new Guid("eb9ecb38-c6e9-42d9-9295-ce2c307b0181")
                        },
                        new
                        {
                            UserId = new Guid("83c85d03-cb46-4ebb-b52f-315eaad91ede"),
                            RoleId = new Guid("4418aba5-5187-4c26-b0bc-c41ced161e03")
                        },
                        new
                        {
                            UserId = new Guid("167326c1-11c3-494c-aa9b-9e0e42ee4837"),
                            RoleId = new Guid("166a4262-32fa-4368-898d-d41e2cb08fb6")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entity.Electric", b =>
                {
                    b.HasOne("Core.Data.Entity.Building", "Building")
                        .WithMany("Electrics")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Data.Entity.SchoolInfo", null)
                        .WithMany("Electrics")
                        .HasForeignKey("SchoolInfoId");

                    b.Navigation("Building");
                });

            modelBuilder.Entity("Core.Data.Entity.NaturalGas", b =>
                {
                    b.HasOne("Core.Data.Entity.Building", "Building")
                        .WithMany("NaturalGasUsages")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("Core.Data.Entity.Paper", b =>
                {
                    b.HasOne("Core.Data.Entity.SchoolInfo", "SchoolInfo")
                        .WithMany("Papers")
                        .HasForeignKey("SchoolInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SchoolInfo");
                });

            modelBuilder.Entity("Core.Data.Entity.Water", b =>
                {
                    b.HasOne("Core.Data.Entity.SchoolInfo", "SchoolInfo")
                        .WithMany("Waters")
                        .HasForeignKey("SchoolInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SchoolInfo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Core.Data.Entity.User.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Core.Data.Entity.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Core.Data.Entity.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Core.Data.Entity.User.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Data.Entity.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Core.Data.Entity.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Data.Entity.Building", b =>
                {
                    b.Navigation("Electrics");

                    b.Navigation("NaturalGasUsages");
                });

            modelBuilder.Entity("Core.Data.Entity.SchoolInfo", b =>
                {
                    b.Navigation("Electrics");

                    b.Navigation("Papers");

                    b.Navigation("Waters");
                });
#pragma warning restore 612, 618
        }
    }
}
