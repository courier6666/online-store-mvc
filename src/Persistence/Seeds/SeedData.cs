using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.DatabaseContexts;

namespace Store.Persistence.Main.Seeds
{
    public static class SeedData
    {
        public static void Seed(string connectionString)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().
                UseSqlServer(connectionString).
                Options;

            using var context = new ApplicationDbContext(options);
            context.Products.AddRange(new List<Product>
            {
                new Product()
                {
                    Name = "Product1",
                    Category = "Category1",
                    Description = "Desc1",
                    Price = 15.00m,
                },
            });

            context.SaveChanges();
        }
    }
}
