namespace Store.Domain.Entities.Interfaces
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        Guid? UserId { get; }
    }
}
