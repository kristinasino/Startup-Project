using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Dto.Tenants;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.Tenants.GetById
{
    public record TenantGetByIdQuery(int Id) : IRequest<TenantDto>;
    
    
    public class TenantGetByIdHandler : BaseService, IRequestHandler<TenantGetByIdQuery, TenantDto>
    {
        public TenantGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<TenantDto> Handle(TenantGetByIdQuery request, CancellationToken cancellationToken)
        {
            var tenant = await UnitOfWork.GetRepository<Tenant>().AsTableNoTracking
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (tenant == null)
                throw new BadRequestException("Tenant is not found!");
            
            return Mapper.Map<TenantDto>(tenant);
        }
    }
}