using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Dto.CustomerEquipment;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Commands.CustomerEquipment.Update;

public record CustomerEquipmentUpdateCommand(CustomerEquipmentUpdateDto EquipmentUpdateDto) : IRequest;

public class CustomerEquipmentUpdateHandler : BaseService, IRequestHandler<CustomerEquipmentUpdateCommand>
{
    public CustomerEquipmentUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,
    mapper)
    {
    }

    public async Task Handle(CustomerEquipmentUpdateCommand request, CancellationToken cancellationToken)
    {
        var equipmentUpdateDto = request.EquipmentUpdateDto;

        var equipment = await UnitOfWork.GetRepository<CustomerLocationEquipment>()
            .AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.EquipmentUpdateDto.Id, cancellationToken: cancellationToken);

        if (equipment == null)
            throw new NotFoundException("Equipment was not found");

        Mapper.Map(equipmentUpdateDto, equipment);

        await UnitOfWork.CommitAsync();
    }
}
