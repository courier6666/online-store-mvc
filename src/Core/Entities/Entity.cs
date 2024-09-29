using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Domain.Entities.Interfaces;

namespace Store.Domain.Entities
{
    /// <summary>
    /// Base entity used for defining all other entities of domain. Includes basic properties that all entities need to have.
    /// </summary>
    /// <typeparam name="TEntityId">Type of id for entity. Used as a primary key</typeparam>
    public abstract class Entity<TEntityId> : IEntity<TEntityId>
    where TEntityId : struct
    {
        public TEntityId Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] Version { get; set; }
        /// <summary>
        /// Initializes CreateDate property of entity.
        /// </summary>
        public Entity()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
