using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.CustomerLocation;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.CustomerLocations.List
{
    public record CustomerLocationsListQuery(int? TenantId) : IRequest<IEnumerable<CustomerLocationDto>>;
    
    
    public class CustomerLocationsListHandler : BaseService, IRequestHandler<CustomerLocationsListQuery, IEnumerable<CustomerLocationDto>>
    {
        public CustomerLocationsListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<CustomerLocationDto>> Handle(CustomerLocationsListQuery request, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUser();
            var query = UnitOfWork.GetRepository<CustomerLocation>()
                .AsTableNoTracking;
            
            if (user.TenantId is not null)
                query = query.Where(t => t.TenantId == user.TenantId);
            else if(request.TenantId is not null)
                query = query.Where(t => t.TenantId == request.TenantId);

            
            return Mapper.Map<IEnumerable<CustomerLocationDto>>(query);
        }
    }
}