
namespace ERP_API.DTOS.ReportStatistic
{
      public class PagedResultDTO<T>
      {
            public List<T> Data { get; set; }
            public int TotalRecords { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
            public bool HasPrevious => PageNumber > 1;
            public bool HasNext => PageNumber < TotalPages;
      }
}
