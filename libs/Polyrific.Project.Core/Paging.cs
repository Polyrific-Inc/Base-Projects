using System.Collections.Generic;

namespace Polyrific.Project.Core
{
    public class Paging<T> where T : class
    {
        public Paging()
        {
            Items = new List<T>();
        }

        public Paging(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
