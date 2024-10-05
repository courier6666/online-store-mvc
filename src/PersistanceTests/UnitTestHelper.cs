using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.DatabaseContexts;
using Store.Persistence.Main.Identity;

namespace PersistanceTests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<ApplicationDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                SeedData(context);
            }

            return options;
        }
        public static void SeedData(ApplicationDbContext applicationDbContext)
        {
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "William",
                Birthday = new DateOnly(2000, 7, 7),
                Email = "123@gmail.com",
                PasswordHash = "1",
                Login = "123434"
            };

            Address address = new Address()
            {
                PersonId = user.Id,
                Country = "1",
                City = "2",
                State = "3",
                Street = "4",
            };

            applicationDbContext.Addresses.Add(address);
            applicationDbContext.Users.Add(user);
            applicationDbContext.SaveChanges();
        }
    }
}
