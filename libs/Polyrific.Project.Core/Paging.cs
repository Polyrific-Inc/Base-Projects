using System.Collections.Generic;

namespace Polyrific.Project.Core
{
    /// <summary>
    /// Container to wrap a paging collection of entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paging<T> where T : class
    {
        /// <summary>
        /// Instantiate an empty paging collection
        /// </summary>
        public Paging()
        {
            Items = new List<T>();
        }

        /// <summary>
        /// Intantiate Paging collection
        /// </summary>
        /// <param name="items">Collection of the entities</param>
        /// <param name="totalCount">Total number of overall entities</param>
        /// <param name="page">Current page number</param>
        /// <param name="pageSize">The maximum number of entities in a page</param>
        public Paging(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// Collection of the entities
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Total number of overall entities
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The maximum number of entities in a page
        /// </summary>
        public int PageSize { get; set; }
    }
}
