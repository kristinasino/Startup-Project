using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Identity.Microservice.Core.Dto.Contract;
using Identity.Microservice.Core.Services;
using MediatR;
using Shared.Infrastructure.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Commands.Contract.Create;
public record CustomerContractCreateCommand(ContractCreateDto ContractCreateDto) : IRequest;
public class CustomerContractCreateHandler : BaseService, IRequestHandler<CustomerContractCreateCommand>
{
    public CustomerContractCreateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    { }

    public async Task Handle(CustomerContractCreateCommand request, CancellationToken cancellationToken)
    {
        var contractCreateDto = request.ContractCreateDto;

        await UnitOfWork.GetRepository<CustomerContract>().AddAsync(Mapper.Map<CustomerContract>(contractCreateDto));

        await UnitOfWork.CommitAsync();
    }
}

public sealed class CustomerContractCreateCommandValidator : AbstractValidator<CustomerContractCreateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerContractCreateCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ContractCreateDto.ContractTypeId)
            .NotEmpty()
            .WithMessage("Contract type is required");
        RuleFor(x => x.ContractCreateDto.CustomerLocationId)
            .NotEmpty()
            .WithMessage("Contract location is required");
        RuleFor(x => x.ContractCreateDto.StartDate)
            .NotEmpty()
            .WithMessage("Start Date is required");
        RuleFor(x => x.ContractCreateDto.EndDate)
            .NotEmpty()
            .WithMessage("End Date is required");
    }
}

