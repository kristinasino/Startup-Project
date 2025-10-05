using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Microservice.Core.Dto.FilterPagination;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Queries.Users.List
{
    public record UserListQuery(UserFilterDto UserFilterDto) : IRequest<PaginateDto<UserDto>>;
    
    
    public class UserListHandler : BaseService, IRequestHandler<UserListQuery, PaginateDto<UserDto>>
    {
        public UserListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<PaginateDto<UserDto>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var loggedInUser = await GetCurrentUser();
            var isAdmin = loggedInUser.UserRoles.First().Role.Name == "Administrator";
            var userFilterDto = request.UserFilterDto;
            var users = await UnitOfWork.GetRepository<User>().AsTableNoTracking
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Where(x => isAdmin || x.TenantId == loggedInUser.TenantId)
                .PaginateAsync(userFilterDto.PageNumber, userFilterDto.PageSize, userFilterDto.SortBy, userFilterDto.Order, cancellationToken: cancellationToken);
            return Mapper.Map<PaginateDto<UserDto>>(users);
        }
    }
}