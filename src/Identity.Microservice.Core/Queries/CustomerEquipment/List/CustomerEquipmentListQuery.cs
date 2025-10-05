using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Dto.Contract;
using Identity.Microservice.Core.Dto.CustomerEquipment;
using Identity.Microservice.Core.Queries.Contract.List;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.CustomerEquipment.List;

public record CustomerEquipmentListQuery : IRequest<IEnumerable<CustomerEquipmentDto>>;


public class CustomerEquipmentListHandler : BaseService, IRequestHandler<CustomerEquipmentListQuery, IEnumerable<CustomerEquipmentDto>>
{
    public CustomerEquipmentListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
    { }

    public async Task<IEnumerable<CustomerEquipmentDto>> Handle(CustomerEquipmentListQuery request, CancellationToken cancellationToken)
    {
        var contracts = await UnitOfWork.GetRepository<CustomerLocationEquipment>().AsTableNoTracking
            .Include(x => x.CustomerLocation)
            .Include(x => x.EquipmentType)
            .Include(x => x.ServiceType)
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<CustomerEquipmentDto>>(contracts);
    }
}
