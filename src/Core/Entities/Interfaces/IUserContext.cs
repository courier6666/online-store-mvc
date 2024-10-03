using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Interfaces
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        Guid? UserId { get; }
    }
}
