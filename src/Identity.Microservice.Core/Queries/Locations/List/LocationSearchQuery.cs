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

namespace Identity.Microservice.Core.Queries.Locations.List;

public record LocationSearchQuery(string SearchTerm) : IRequest<IEnumerable<LocationDto>>;
    
    
public class LocationSearchHandler : BaseService, IRequestHandler<LocationSearchQuery, IEnumerable<LocationDto>>
{
    public LocationSearchHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
    { }

    public async Task<IEnumerable<LocationDto>> Handle(LocationSearchQuery request, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUser();
        var query = UnitOfWork.GetRepository<CustomerLocation>()
            .AsTableNoTracking;

        if (user.TenantId is not null)
            query = query.Where(t => t.TenantId == user.TenantId);

        var locations = query
            .Where(x => x.Name.Contains(request.SearchTerm))
            .Take(10)
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<LocationDto>>(locations);
    }
}