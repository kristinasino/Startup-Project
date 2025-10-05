using Shared.Entities.Enums;

namespace Shared.Common.Dto.FilterPagination
{
    public class BasePagedFilterDto
    {
        private const int maxPageSize = 1000;
        private int _pageSize = 15;
        private int _pageNumber = 1;

        public string? SortBy { get; set; }
        public OrderEnum Order { get; set; }

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value <= 0 ? _pageNumber : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value <= 0 ? _pageSize : value;
        }
    }
}