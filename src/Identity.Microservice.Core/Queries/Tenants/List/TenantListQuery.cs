using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Tenants;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Queries.Tenants.List
{
    public record TenantListQuery : IRequest<IEnumerable<TenantDto>>;
    
    
    public class TenantListHandler : BaseService, IRequestHandler<TenantListQuery, IEnumerable<TenantDto>>
    {
        public TenantListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<TenantDto>> Handle(TenantListQuery request, CancellationToken cancellationToken)
        {
            var tenants = await UnitOfWork.GetRepository<Tenant>().AsTableNoTracking
                .ToListAsync(cancellationToken: cancellationToken);
            return Mapper.Map<IEnumerable<TenantDto>>(tenants);
        }
    }
}