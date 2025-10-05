using Domain.Entities;
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

namespace Identity.Microservice.Core.Commands.Location.Delete;

public record CustomerLocationDeleteCommand(int id): IRequest;
    
public class CustomerLocationDeleteHandler: BaseService, IRequestHandler<CustomerLocationDeleteCommand>
{
    public CustomerLocationDeleteHandler(IUnitOfWork unitOfWork):base(unitOfWork)
    { }

    public async Task Handle(CustomerLocationDeleteCommand request, CancellationToken cancellationToken)
    {
        var query = UnitOfWork.GetRepository<CustomerLocation>().AsTrackingEntity;
        var user = await GetCurrentUser();

        if (user.TenantId is not null)
            throw new BadRequestException("Invalid tenant.");

        if (user.TenantId is not null)
            query = query.Where(t => t.TenantId == user.TenantId);

        var location = await UnitOfWork.GetRepository<CustomerLocation>().AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken: cancellationToken);

        if (location == null)
            throw new NotFoundException("Location not found!");


        UnitOfWork.GetRepository<CustomerLocation>().SoftDelete(location);


        await UnitOfWork.CommitAsync();
    }
}

