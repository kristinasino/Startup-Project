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
using Identity.Microservice.Domain.Entities;

namespace Identity.Microservice.Core.Commands.Tenants.Delete
{
   
    public record TenantDeleteCommand(int id) : IRequest;

    public class TenantDeleteHandler : BaseService, IRequestHandler<TenantDeleteCommand>
    {
        public TenantDeleteHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public async Task Handle(TenantDeleteCommand request, CancellationToken cancellationToken)
        {
            var tenant = await UnitOfWork.GetRepository<Tenant>().AsTrackingEntity
                .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken: cancellationToken);

            if (tenant == null)
                throw new NotFoundException("Tenant not found!");


            tenant.IsSoftDeleted = true;


            await UnitOfWork.CommitAsync();
        }
    }
}
