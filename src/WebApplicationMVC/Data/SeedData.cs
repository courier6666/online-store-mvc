using Microsoft.AspNetCore.Identity;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.DatabaseContexts;

namespace Store.WebApplicationMVC.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Products.Any())
            {
                string[] categories = new[] {
                "Food",
                "Clothing",
                "Furniture",
                "Electrical devices",
                "Alcohol"
            };

                Random rnd = new Random();

                for (int i = 0; i < 10000; i++)
                {
                    context.Add(new Product()
                    {
                        Name = $"Product{i}",
                        Category = categories[i % categories.Length],
                        Description = $"Product{i} of category - '{categories[i % categories.Length]}'",
                        Price = rnd.Next(5, 1000) + .5M
                    });
                }
            }
            context.SaveChanges();
        }
        public static async Task SeedRolesAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var role in Roles.GetAllRoles().ToArray())
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
