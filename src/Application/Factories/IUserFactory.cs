using Store.Domain.Entities.Interfaces;

namespace Store.Application.Factories
{
    public interface IUserFactory
    {
        public IUser CreateNewEmptyUser();
    }
}
