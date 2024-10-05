//#define MIGRATION

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.Extensions;
using Store.Persistence.Main.Identity;
using System.Reflection;

namespace Store.Persistence.Main.DatabaseContexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
# if MIGRATION
        public ApplicationDbContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(localDb)\\MSSQLLocalDB;Initial Catalog=StoreDb;Integrated Security=True;Connect Timeout=30;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
# endif
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
