using Microsoft.CodeAnalysis;

namespace ERP_API.Models
{
    public class PagedList<T> : List<T>
    {
        public PagedMetaData MetaData { get; private set; }

        public PagedList(IEnumerable<T> items, long totalItems, int pageIndex, int pageSize)
        {
            MetaData = new PagedMetaData
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageIndex
            };
            AddRange(items);
        }

        public PagedMetaData GetMetaData() => MetaData;
    }

}
