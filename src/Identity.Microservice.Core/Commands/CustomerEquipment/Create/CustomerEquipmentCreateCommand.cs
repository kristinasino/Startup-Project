using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Dto.CustomerEquipment;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Commands.CustomerEquipment.Create;
public record CustomerEquipmentCreateCommand(CustomerEquipmentCreateDto EquipmentCreateDto) : IRequest;
public class CustomerEquipmentCreateHandler : BaseService, IRequestHandler<CustomerEquipmentCreateCommand>
{
    public CustomerEquipmentCreateHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
    { }

    public async Task Handle(CustomerEquipmentCreateCommand request, CancellationToken cancellationToken)
    {
        var equipmentCreateDto = request.EquipmentCreateDto;

        var entity = Mapper.Map<CustomerLocationEquipment>(equipmentCreateDto);
        
        await UnitOfWork.GetRepository<CustomerLocationEquipment>().AddAsync(entity);

        await UnitOfWork.CommitAsync();
    }
}

// public sealed class CustomerEquipmentCreateCommandValidator : AbstractValidator<CustomerEquipmentCreateCommand>
// {
//     private readonly IUnitOfWork _unitOfWork;
//
//     public CustomerEquipmentCreateCommandValidator(IUnitOfWork unitOfWork)
//     {
//         _unitOfWork = unitOfWork;
//
//         RuleFor(x => x.EquipmentCreateDto.EquipmentTypeId)
//             .NotEmpty()
//             .WithMessage("Equipment type is required");
//         RuleFor(x => x.EquipmentCreateDto.CustomerLocationId)
//             .NotEmpty()
//             .WithMessage("Equipment location is required");
//         RuleFor(x => x.EquipmentCreateDto.StartDate)
//             .NotEmpty()
//             .WithMessage("Start Date is required");
//         RuleFor(x => x.EquipmentCreateDto.EndDate)
//             .NotEmpty()
//             .WithMessage("End Date is required");
//     }
// }

