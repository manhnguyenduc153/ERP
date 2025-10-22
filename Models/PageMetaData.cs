namespace ApiWithRoles.Models
{
    public class PagedMetaData
    {
        public long TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrev => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }

}
