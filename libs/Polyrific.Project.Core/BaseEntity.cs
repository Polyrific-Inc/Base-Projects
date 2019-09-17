using System;

namespace Polyrific.Project.Core
{
    /// <summary>
    /// The entity base class
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Model Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Model created date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Model updated date
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// A random value that must change whenever an entity is persisted
        /// </summary>
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
