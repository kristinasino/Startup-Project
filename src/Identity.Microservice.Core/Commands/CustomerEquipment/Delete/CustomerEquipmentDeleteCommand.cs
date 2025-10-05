using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Commands.CustomerEquipment.Delete;
public record CustomerEquipmentDeleteCommand(int Id) : IRequest;

public class CustomerEquipmentDeleteHandler : BaseService, IRequestHandler<CustomerEquipmentDeleteCommand>
{
    public CustomerEquipmentDeleteHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    { }

    public async Task Handle(CustomerEquipmentDeleteCommand request, CancellationToken cancellationToken)
    {
        var equipment = await UnitOfWork.GetRepository<CustomerLocationEquipment>().AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (equipment == null)
            throw new NotFoundException("Equipment not found!");


        UnitOfWork.GetRepository<CustomerLocationEquipment>().SoftDelete(equipment);


        await UnitOfWork.CommitAsync();
    }
}

