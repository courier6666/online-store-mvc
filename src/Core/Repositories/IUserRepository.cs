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
        /// <summary>
        /// Find user <typeparamref name="User"/> by login.
        /// </summary>
        /// <param name="login">Login of searched user.</param>
        /// <returns>Found user <typeparamref name="User"/> by login</returns>
        Task<IUser> FindUserByLoginAsync(string login);
    }
}
