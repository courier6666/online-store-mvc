using Store.Application.Interfaces.Services;
using Store.Application.Services;
using Store.Infrastructure.Mappers;
using Store.Application.Interfaces.Mapper;
using Store.Persistence.Main.DatabaseContexts;
using Store.Persistence.Main.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Interfaces;
using Store.Persistence.Main.Identity;
using Microsoft.AspNetCore.Identity;
using Store.WebApplicationMVC.Data;
using Store.Application.Factories;
using Store.Persistence.Main.Factories;
using Store.WebApplicationMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICustomMapper>(CustomMapperFactory.Create());
builder.Services.AddSingleton<IUserFactory, UserFactory>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDb"));
});

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
builder.Services.AddScoped<ICashDepositService, CashDepositService>();
builder.Services.AddScoped<IOrderHistoryService, OrderHistoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserOrderService, UserOrderService>();
builder.Services.AddScoped<ICartService>(SessionCartService.GetCart);

builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
}

app.UseSession();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{page:int}",
    defaults: new { Controller = "Home", Action="Index", page = 1 }
    );

app.MapControllerRoute(
    name: "paginationNewest",
    pattern: "Products/Newest/Page{page:int}",
    defaults: new { Controller = "Home", Action = "Index", page = 1, order = "newest" }
    );

app.MapControllerRoute(
    name: "paginationOldest",
    pattern: "Products/Oldest/Page{page:int}",
    defaults: new { Controller = "Home", Action = "Index", page = 1, order = "oldest" }
    );

app.MapControllerRoute(
    name: "paginationCheapest",
    pattern: "Products/Cheapest/Page{page:int}",
    defaults: new { Controller = "Home", Action = "Index", page = 1, order = "cheapest" }
    );

app.MapControllerRoute(
    name: "paginationPriciest",
    pattern: "Products/Priciest/Page{page:int}",
    defaults: new { Controller = "Home", Action = "Index", page = 1, order = "priciest" }
    );

app.MapDefaultControllerRoute();

//SeedData.EnsurePopulated(app);
await SeedData.SeedRolesAsync(app);

app.Run();