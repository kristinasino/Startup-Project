using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Tenants;
using Identity.Microservice.Core.Services;
using MediatR;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Domain.Entities;

namespace Identity.Microservice.Core.Commands.Tenants.Create
{
    public record TenantCreateCommand(TenantCreateDto TenantCreateDto): IRequest;
    
    public class TenantCreateHandler: BaseService, IRequestHandler<TenantCreateCommand>
    { 
        public  TenantCreateHandler(IUnitOfWork unitOfWork, IMapper mapper) :base(unitOfWork, mapper)
        { }

        public async Task Handle(TenantCreateCommand request, CancellationToken cancellationToken)
        {
            var tenantCreateDto = request.TenantCreateDto;
            var newLocation = await UnitOfWork.GetRepository<Tenant>().AddAsync(Mapper.Map<Tenant>(tenantCreateDto));

            await UnitOfWork.CommitAsync();
        }
    }

}
