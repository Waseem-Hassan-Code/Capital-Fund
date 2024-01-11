using Capital.Funds.Models;
using Capital.Funds.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;

namespace Capital.Funds.Database
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string Salt = SD.GenerateSalt();
            string HashedPassword = SD.HashPassword("admin", Salt);

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Capital Fund",
                    Email = "admin@admin.com",
                    Password = HashedPassword,
                    Salt = Salt,
                    Gender = "Male",
                    Role = "admin",
                    OTP = "112233",
                    isEmailVerified = true,
                    IsActive = true,
                }
            ); 

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<TenantComplaints> TenantComplaints { get; set; }
        public DbSet<TenatDetails> TenatDetails { get; set;}
        public DbSet<TenantPayments> TenantPayments { get; set;}
        public DbSet<PropertyDetails> PropertyDetails { get; set; }
        public DbSet<ComplaintFiles> ComplaintFiles { get; set; }
    }
}
