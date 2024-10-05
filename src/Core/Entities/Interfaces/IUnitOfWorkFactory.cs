namespace Store.Domain.Entities.Interfaces
{
    /// <summary>
    /// Abstract factory, used for creating unit of works.
    /// Unit of work is created in a separate context.
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
