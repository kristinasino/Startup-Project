using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Configurables;
using Identity.Microservice.Core.Dto.Contract;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.Configurables;

public record ServiceTypeListQuery : IRequest<IEnumerable<ServiceTypeDto>>;

public class ServiceTypeListHandler : BaseService, IRequestHandler<ServiceTypeListQuery, IEnumerable<ServiceTypeDto>>
{
    public ServiceTypeListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) :
        base(unitOfWork, mapper, httpContextAccessor)
    {
    }

    public async Task<IEnumerable<ServiceTypeDto>> Handle(ServiceTypeListQuery request,
        CancellationToken cancellationToken)
    {
        var serviceType = await UnitOfWork.GetRepository<ServiceType>().AsTableNoTracking
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<ServiceTypeDto>>(serviceType);
    }
}