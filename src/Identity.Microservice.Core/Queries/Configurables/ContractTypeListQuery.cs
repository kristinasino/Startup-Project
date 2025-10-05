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

public record ContractTypeListQuery : IRequest<IEnumerable<ContractTypeDto>>;

public class ContractTypeListHandler : BaseService, IRequestHandler<ContractTypeListQuery, IEnumerable<ContractTypeDto>>
{
    public ContractTypeListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) :
        base(unitOfWork, mapper, httpContextAccessor)
    {
    }

    public async Task<IEnumerable<ContractTypeDto>> Handle(ContractTypeListQuery request, CancellationToken cancellationToken)
    {
        var contractType = await UnitOfWork.GetRepository<ContractType>().AsTableNoTracking
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<ContractTypeDto>>(contractType);
    }
}