using System;

namespace Polyrific.Project.Core
{
    /// <summary>
    /// The entity base class
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date when this entity was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The user who initially created this entity (optional).
        /// It is recommended to use this format for uniformity: DisplayName | Email
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The last date when this entity was updated (optional).
        /// If the value is empty, it means that this entity has never been updated.
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// The last user who updated this entity (optional).
        /// It is recommended to use this format for uniformity: DisplayName | Email
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// A random value that must change whenever an entity is persisted
        /// </summary>
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Update the properties of this entity with the values from the source entity
        /// <para>(This method need to be implemented on each entity to explicitly define which properties
        /// that will be copied)<para>
        /// </summary>
        /// <param name="source">The source entity</param>
        public abstract void UpdateValueFrom(BaseEntity source);
    }
}
