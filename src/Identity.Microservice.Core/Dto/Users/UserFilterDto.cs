using Shared.Common.Dto.FilterPagination;

namespace Identity.Microservice.Core.Dto.Users
{
    public class UserFilterDto : BasePagedFilterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}