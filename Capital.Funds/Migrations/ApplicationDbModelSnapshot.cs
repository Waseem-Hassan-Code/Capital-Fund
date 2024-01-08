﻿// <auto-generated />
using System;
using Capital.Funds.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Capital.Funds.Migrations
{
    [DbContext(typeof(ApplicationDb))]
    partial class ApplicationDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Capital.Funds.Models.PropertyDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberofBathrooms")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberofBedrooms")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeofProperty")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isAvailable")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PropertyDetails");
                });

            modelBuilder.Entity("Capital.Funds.Models.TenantComplaints", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFixed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TenantComplaints");
                });

            modelBuilder.Entity("Capital.Funds.Models.TenantPayments", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AreaMaintainienceFee")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("LateFee")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rent")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RentPayedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("isLate")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isPayable")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TenantPayments");
                });

            modelBuilder.Entity("Capital.Funds.Models.TenatDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MovedIn")
                        .HasColumnType("TEXT");

                    b.Property<string>("MovedOut")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PropertyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TenatDetails");
                });

            modelBuilder.Entity("Capital.Funds.Models.Users", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OTP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("isEmailVerified")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "1e8f8220-2ee8-4241-9647-916482206cb1",
                            Email = "admin@admin.com",
                            Gender = "Male",
                            IsActive = true,
                            Name = "Capital Fund",
                            OTP = "112233",
                            Password = "VUct4jIlYf5wizQWudIuGblzgBNCLgEwGj6CLHH71o4=",
                            Role = "admin",
                            Salt = "1CtsbaD8pa2uZqIrrgqd4vfjO8cu84vTtZki2FlIi/U=",
                            isEmailVerified = true
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
