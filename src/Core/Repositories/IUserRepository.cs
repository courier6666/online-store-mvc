using Store.Domain.Entities.Interfaces;

namespace Store.Domain.Repositories
{
    public interface IUserRepository : IRepository<IUser, Guid>
    {

    }
}
