using InvoiceManagementApp.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace InvoiceManagementApp.Models.ApplicationContext
{
    public class ApplicationContextDbContext : DbContext
    {
        public ApplicationContextDbContext()
        { }
        public ApplicationContextDbContext(DbContextOptions<ApplicationContextDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItems");
        }
    }
}
