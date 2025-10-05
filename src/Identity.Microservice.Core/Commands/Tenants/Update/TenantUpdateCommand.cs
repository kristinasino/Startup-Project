using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Tenants;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Domain.Entities;

namespace Identity.Microservice.Core.Commands.Tenants.Update;

    
public record TenantUpdateCommand(TenantDto TenantDto) : IRequest;

public class TenantUpdateHandler : BaseService, IRequestHandler<TenantUpdateCommand>
{
    public TenantUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,
    mapper)
    {
    }

    public async Task Handle(TenantUpdateCommand request, CancellationToken cancellationToken)
    {
        var tenantDto = request.TenantDto;

        var tenant = await UnitOfWork.GetRepository<Tenant>()
            .AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.TenantDto.Id, cancellationToken: cancellationToken);

        if (tenant == null)
            throw new NotFoundException("Tenant was not found");

        Mapper.Map(tenantDto, tenant);

        await UnitOfWork.CommitAsync();
    }
}



