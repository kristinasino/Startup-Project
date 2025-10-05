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

namespace Identity.Microservice.Core.Commands.Contract.Delete;
public record CustomerContractDeleteCommand(int id) : IRequest;

public class CustomerContractDeleteHandler : BaseService, IRequestHandler<CustomerContractDeleteCommand>
{
    public CustomerContractDeleteHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    { }

    public async Task Handle(CustomerContractDeleteCommand request, CancellationToken cancellationToken)
    {
        var contract = await UnitOfWork.GetRepository<CustomerContract>().AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken: cancellationToken);

        if (contract == null)
            throw new NotFoundException("Contract not found!");


        UnitOfWork.GetRepository<CustomerContract>().SoftDelete(contract);


        await UnitOfWork.CommitAsync();
    }
}

