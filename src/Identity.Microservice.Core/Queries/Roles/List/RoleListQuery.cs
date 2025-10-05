using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto.Role;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Queries.Roles.List
{
    public record RoleListQuery : IRequest<IEnumerable<RoleDto>>;
    
    
    public class RoleListHandler : BaseService, IRequestHandler<RoleListQuery, IEnumerable<RoleDto>>
    {
        public RoleListHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<IEnumerable<RoleDto>> Handle(RoleListQuery request, CancellationToken cancellationToken)
        {
            var roles = await UnitOfWork.GetRepository<Role>().AsTableNoTracking.ToListAsync(cancellationToken: cancellationToken);
            return Mapper.Map<IEnumerable<RoleDto>>(roles);
        }
    }
}