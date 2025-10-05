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

public record VendorTypeListQuery : IRequest<IEnumerable<VendorDto>>;

public class VendorTypeListHandler : BaseService, IRequestHandler<VendorTypeListQuery, IEnumerable<VendorDto>>
{
    public VendorTypeListHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) :
        base(unitOfWork, mapper, httpContextAccessor)
    {
    }

    public async Task<IEnumerable<VendorDto>> Handle(VendorTypeListQuery request, CancellationToken cancellationToken)
    {
        var serviceType = await UnitOfWork.GetRepository<Vendor>().AsTableNoTracking
            .ToListAsync(cancellationToken: cancellationToken);
        return Mapper.Map<IEnumerable<VendorDto>>(serviceType);
    }
}