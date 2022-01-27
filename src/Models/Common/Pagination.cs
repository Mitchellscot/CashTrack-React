using System.Collections.Generic;

namespace CashTrack.Models.Common
{
    public abstract class PaginationRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string Query { get; set; } = null;
    }
    public abstract class PaginationResponse<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 25;
        public int TotalPages { get; set; }
        public decimal TotalCount { get; set; }
        public IEnumerable<T> ListItems { get; set; }
    }
}
