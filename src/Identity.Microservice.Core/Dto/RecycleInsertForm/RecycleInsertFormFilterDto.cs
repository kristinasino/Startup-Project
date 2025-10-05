using Shared.Common.Dto.FilterPagination;

namespace Identity.Microservice.Core.Dto
{
    public class RecycleInsertFormFilterDto : BasePagedFilterDto
    {
        public int? TenantId { get; set; }
        public int? CustomerLocationId { get; set; }
        public int[] Month { get; set; }
        public int[] Year { get; set; }
    }
}
