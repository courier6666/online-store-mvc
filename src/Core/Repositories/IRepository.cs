namespace Store.Domain.Repositories
{
    /// <summary>
    /// Used for basic repository operations.
    /// </summary>
    /// <typeparam name="TObject">Entity stored in repository.</typeparam>
    public interface IRepository<TObject> : IDisposable
    where TObject : class
    {
        /// <summary>
        /// Used for getting all entities in repository.
        /// </summary>
        /// <returns>Collections of all entities in repository.</returns>
        Task<IEnumerable<TObject>> GetAllAsync();
        /// <summary>
        /// Used for adding entity into repository.
        /// </summary>
        /// <param name="entity"> Entity that is added.</param>
        void Add(TObject entity);
        /// <summary>
        /// Used for updating entity in repository.
        /// </summary>
        /// <param name="entity">Entity that is updated.</param>
        void Update(TObject entity);
        /// <summary>
        /// Used for deleting entity from repository.
        /// </summary>
        /// <param name="entity">Entity that is deleted.</param>
        void Delete(TObject entity);
    }
    /// <summary>
    /// Used to get Entity <typeparamref name="TObject"/> by id
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TObjectId">Id of entity of type <typeparamref name="TObject"/></typeparam>
    public interface IRepository<TObject, TObjectId> : IRepository<TObject>
    where TObject : class
    {
        /// <summary>
        /// Used to find entity by id.
        /// </summary>
        /// <param name="id">Id of entity to find</param>
        /// <returns>Entity of type <typeparamref name="TObject"/></returns>
        Task<TObject> GetByIdAsync(TObjectId id);
    }
}