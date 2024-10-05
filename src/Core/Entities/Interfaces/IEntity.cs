namespace Store.Domain.Entities.Interfaces
{

    public interface IEntity<TEntityId>
    where TEntityId : struct
    {
        public TEntityId Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        byte[] Version { get; set; }
    }
}
