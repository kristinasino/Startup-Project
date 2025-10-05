using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Contract;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Queries.Contract.List;

public record ContractListQuery : IRequest<IEnumerable<ContractDto>>;

public class ContractListHandler : BaseService, IRequestHandler<ContractListQuery, IEnumerable<ContractDto>>
{
    public ContractListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
    { }

    public async Task<IEnumerable<ContractDto>> Handle(ContractListQuery request, CancellationToken cancellationToken)
    {
        var contracts = await UnitOfWork.GetRepository<CustomerContract>().AsTableNoTracking
            .Include(x => x.Vendor)
            .Include(x => x.CustomerLocation)
            .Include(x => x.ContractType)
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<ContractDto>>(contracts);
    }
}
