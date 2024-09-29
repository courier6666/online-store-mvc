using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Factories
{
    public interface IUserFactory
    {
        public IUser CreateNewEmptyUser();
    }
}
