using Moq;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Domain.Entities;
using Store.Domain.PagedLists;

namespace Store.WebApplicationMVC.Moq
{
    public class MoqFactory
    {
        public static IProductService GetMoqProductService(IServiceProvider serviceProvider)
        {
            string[] categories = new[] {
                "Food",
                "Clothing",
                "Furniture",
                "Electrical devices",
                "Alcohol" };

            Random rnd = new Random();
            List<ProductDto> products = new List<ProductDto>();
            for (int i = 0; i < 10000; i++)
            {
                products.Add(new ProductDto()
                {
                    Name = $"Product{i}",
                    Category = categories[i % categories.Length],
                    Description = $"Product{i} of category - '{categories[i % categories.Length]}'",
                    Price = rnd.Next(5, 1000) + .5M
                });
            }

            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(ps => ps.GetPagedProductsAsync(It.IsAny<ProductsPageQuery>())).
                Returns(Task.FromResult(new PagedList<ProductDto>(products.Take(20).ToList(), 10000, 1, 20)));
            productServiceMock.Setup(ps => ps.GetAllCategoriesAsync()).
                Returns(Task.FromResult(categories as IEnumerable<string>));

            return productServiceMock.Object;
        }
    }
}
