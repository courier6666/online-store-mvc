using Store.Application.Factories;
using Store.Domain.Entities.Interfaces;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.Factories
{
    public class UserFactory : IUserFactory
    {
        public IUser CreateNewEmptyUser()
        {
            return new AppUser();
        }
    }
}
