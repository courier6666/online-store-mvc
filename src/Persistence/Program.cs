using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Persistence.Main.DatabaseContexts;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;

namespace Store.Persistence
{
    public class Program
    {
        public const string TestDatabaseConnectionString = "Data Source=(localDb)\\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public static async Task DbContextCheck(DbContextOptions<ApplicationDbContext> options)
        {
            using (ApplicationDbContext context = new(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                for (int i = 0; i < 20; ++i)
                {
                    context.Products.Add(new Product()
                    {
                        Name = i.ToString(),
                        Category = i.ToString(),
                        Description = i.ToString(),
                        Price = i + 1
                    });
                }
                context.SaveChanges();

            }
        }
        public static async Task Main(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Data Source=(localDb)\\MSSQLLocalDB;Initial Catalog=StoreDb;Integrated Security=True;Connect Timeout=30;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
                builder =>{
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            await DbContextCheck(builder.Options);
        }
    }
}
