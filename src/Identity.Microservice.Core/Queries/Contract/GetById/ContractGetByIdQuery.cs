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

namespace Identity.Microservice.Core.Queries.Contract.GetById
{
    public record ContractGetByIdQuery(int Id) : IRequest<ContractDto>;


    public class ContractGetByIdHandler : BaseService, IRequestHandler<ContractGetByIdQuery, ContractDto>
    {
        public ContractGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<ContractDto> Handle(ContractGetByIdQuery request, CancellationToken cancellationToken)
        {
            var contract = await UnitOfWork.GetRepository<CustomerContract>().AsTableNoTracking
                .Include(x => x.CustomerLocation)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            return Mapper.Map<ContractDto>(contract);
        }
    }
}
