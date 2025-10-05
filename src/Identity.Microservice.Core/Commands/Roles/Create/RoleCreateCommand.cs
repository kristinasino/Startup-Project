using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto.Role;
using FluentValidation;
using Identity.Microservice.Core.Dto.Role;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Commands.Roles.Create;

public record RoleCreateCommand(RoleCreateDto RoleCreateDto) : IRequest<RoleDto>;

public class RoleCreateHandler : BaseService, IRequestHandler<RoleCreateCommand, RoleDto>
{
    public RoleCreateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,
        mapper)
    {
    }

    public async Task<RoleDto> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var roleCreateDto = request.RoleCreateDto;
        var newRole = await UnitOfWork.GetRepository<Role>().AddAsync(Mapper.Map<Role>(roleCreateDto));

        await UnitOfWork.CommitAsync();

        return Mapper.Map<RoleDto>(newRole.Entity);
    }
}

public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleCreateCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.RoleCreateDto.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MustAsync(async (x, y) => await IsNameTaken(x))
            .WithMessage(x => $"Name {x.RoleCreateDto.Name} is taken");
    }

    private async Task<bool> IsNameTaken(string name)
    {
        var role = await _unitOfWork.GetRepository<Role>().AsTableNoTracking
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower().Replace(" ", ""));

        return role == null;
    }
}