using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Contract;
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

namespace Identity.Microservice.Core.Commands.Contract.Update;

public record CustomerContractUpdateCommand(ContractUpdateDto ContractUpdateDto) : IRequest;

public class CustomerContractUpdateHandler : BaseService, IRequestHandler<CustomerContractUpdateCommand>
{
    public CustomerContractUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,
    mapper)
    {
    }

    public async Task Handle(CustomerContractUpdateCommand request, CancellationToken cancellationToken)
    {
        var contractUpdateDto = request.ContractUpdateDto;

        var contract = await UnitOfWork.GetRepository<CustomerContract>()
            .AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.ContractUpdateDto.Id, cancellationToken: cancellationToken);

        if (contract == null)
            throw new NotFoundException("Contract was not found");

        Mapper.Map(contractUpdateDto, contract);

        await UnitOfWork.CommitAsync();
    }
}
