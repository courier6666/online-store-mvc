using Store.Application.Factories;
using Store.Domain.Entities.Interfaces;
using Store.Persistence.Main.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
