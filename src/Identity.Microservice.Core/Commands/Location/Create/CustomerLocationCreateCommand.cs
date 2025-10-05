using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Services;
using MediatR;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Commands.Location.Create;

public record CustomerLocationCreateCommand(LocationCreateDto LocationCreateDto) : IRequest;
public class CustomerLocationCreateHandler : BaseService, IRequestHandler<CustomerLocationCreateCommand>
{
    public CustomerLocationCreateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    { }

    public async Task Handle(CustomerLocationCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUser();

        if (user.TenantId is not null && request.LocationCreateDto.TenantId != user.TenantId)
            throw new BadRequestException("Invalid tenant.");

        var locationCreateDto = request.LocationCreateDto;
        
        await UnitOfWork.GetRepository<CustomerLocation>().AddAsync(Mapper.Map<CustomerLocation>(locationCreateDto));
        await UnitOfWork.CommitAsync();
    }
}

public sealed class CustomerLocationCreateCommandValidator : AbstractValidator<CustomerLocationCreateCommand>
{
    public CustomerLocationCreateCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.LocationCreateDto.Name)
            .NotEmpty()
            .WithMessage("Name is required");
    }
}