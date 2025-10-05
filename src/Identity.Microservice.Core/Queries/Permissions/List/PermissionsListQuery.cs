using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Dto.Role;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.Permissions.List
{
    public record PermissionsListQuery : IRequest<IEnumerable<PermissionDto>>;
    
    
    public class PermissionsListHandler : BaseService, IRequestHandler<PermissionsListQuery, IEnumerable<PermissionDto>>
    {
        public PermissionsListHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<IEnumerable<PermissionDto>> Handle(PermissionsListQuery request, CancellationToken cancellationToken)
        {
            var permissions = await UnitOfWork.GetRepository<Permission>().AsTableNoTracking.ToListAsync(cancellationToken: cancellationToken);
            return Mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }
    }
}