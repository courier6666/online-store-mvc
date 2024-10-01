using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;

namespace Store.Domain.Repositories
{
    public interface IUserRepository : IRepository<IUser, Guid>
    {

    }
}
