using System.Collections.Generic;

namespace Identity.Microservice.Core.Dto.FilterPagination
{
    public class PaginateDto<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }
}