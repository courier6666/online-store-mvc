using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Persistence.Main.EntityConfigurations;
using Store.Persistence.Main.Extensions;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Store.Persistence.Main.Identity;
using Microsoft.AspNetCore.Identity;

namespace Store.Persistence.Main.DatabaseContexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)));
        }
        public DbSet<CashDeposit> CashDeposits { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProductDetail> OrderProductDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public void Clear()
        {
            CashDeposits.Clear();
            Entries.Clear();
            Orders.Clear();
            OrderProductDetails.Clear();
            Products.Clear();
            Users.Clear();
            Addresses.Clear();
            PaymentDetails.Clear();
            Users.Clear();
            SaveChanges();
        }
    }
}
