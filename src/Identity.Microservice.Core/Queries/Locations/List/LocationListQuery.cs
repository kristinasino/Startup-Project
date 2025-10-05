using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Queries.Locations.List
{
    public record LocationListQuery(int? TenantId) : IRequest<IEnumerable<LocationDto>>;
    
    
    public class LocationListHandler : BaseService, IRequestHandler<LocationListQuery, IEnumerable<LocationDto>>
    {
        public LocationListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<LocationDto>> Handle(LocationListQuery request, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUser();
            var query = UnitOfWork.GetRepository<CustomerLocation>()
                .AsTableNoTracking;

            if (user.TenantId is not null)
                query = query.Where(t => t.TenantId == user.TenantId);
            else if(request.TenantId is not null)
                query = query.Where(t => t.TenantId == request.TenantId);

            var locations = await query .ToListAsync(cancellationToken: cancellationToken);
            return Mapper.Map<IEnumerable<LocationDto>>(locations);
        }
    }
}