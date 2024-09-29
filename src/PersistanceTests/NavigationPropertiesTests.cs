using Microsoft.EntityFrameworkCore;
using Store.Persistence.Main.DatabaseContexts;

namespace PersistanceTests
{
    public class Tests
    {
        [Test]
        public void ApplicationDbCOntext_NavigationProperty_Address_AppUser_IsAppUser()
        {
            //arrange
            var context = new ApplicationDbContext(UnitTestHelper.GetUnitTestDbOptions());
            UnitTestHelper.SeedData(context);

            var expected = context.Users.First();

            //act
            var address = context.Addresses.Include(a => a.Person).First();

            //assert
            Assert.True(address.Person != null && address.Person.Id == expected.Id);
        }
    }
}