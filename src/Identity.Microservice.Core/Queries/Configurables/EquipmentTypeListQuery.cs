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

public record EquipmentTypeListQuery : IRequest<IEnumerable<EquipmentTypeDto>>;

public class EquipmentTypeListHandler : BaseService,
    IRequestHandler<EquipmentTypeListQuery, IEnumerable<EquipmentTypeDto>>
{
    public EquipmentTypeListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) :
        base(unitOfWork, mapper, httpContextAccessor)
    {
    }

    public async Task<IEnumerable<EquipmentTypeDto>> Handle(EquipmentTypeListQuery request,
        CancellationToken cancellationToken)
    {
        var serviceType = await UnitOfWork.GetRepository<EquipmentType>().AsTableNoTracking
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<EquipmentTypeDto>>(serviceType);
    }
}