using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Queries.Locations.GetById
{
    public record LocationGetByIdQuery(int Id) : IRequest<LocationDto>;
    
    
    public class LocationGetByIdHandler : BaseService, IRequestHandler<LocationGetByIdQuery, LocationDto>
    {
        public LocationGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<LocationDto> Handle(LocationGetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUser();
            var query = UnitOfWork.GetRepository<CustomerLocation>()
                .AsTableNoTracking;

            if (user.TenantId is not null)
                query = query.Where(t => t.TenantId == user.TenantId);

            var locations = await UnitOfWork.GetRepository<CustomerLocation>().AsTableNoTracking
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            return Mapper.Map<LocationDto>(locations);
        }
    }
}