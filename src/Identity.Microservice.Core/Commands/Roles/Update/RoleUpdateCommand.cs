using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto.Role;
using FluentValidation;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace UserModule.Core.Commands.Roles.Update;


    public record RoleUpdateCommand(RoleUpdateDto RoleUpdateDto) : IRequest<RoleDto>;
    
    
    public class RoleUpdateHandler : BaseService, IRequestHandler<RoleUpdateCommand, RoleDto>
    {
        
        public RoleUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,
            mapper)
        { }

        public async Task<RoleDto> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var roleUpdateDto = request.RoleUpdateDto;
            
            var role = await UnitOfWork.GetRepository<Role>()
                .AsTrackingEntity
                .Include(x => x.RolePermissions)
                .FirstOrDefaultAsync(x => x.Id == request.RoleUpdateDto.Id, cancellationToken: cancellationToken);

            if (role == null)
                throw new NotFoundException("Role was not found");

            Mapper.Map(roleUpdateDto, role);

            var currentPermissions = role.RolePermissions;

            var newPermissions = roleUpdateDto.Permissions
                .Where(x => !currentPermissions.Select(y => y.PermissionId).Contains(x))
                .Select(x => new RolePermission()
                {
                    RoleId = request.RoleUpdateDto.Id,
                    PermissionId = x
                }).ToList();
            
            var permissionsToDelete = currentPermissions
                .Where(x => !roleUpdateDto.Permissions.Contains(x.PermissionId)).ToList();
            
            permissionsToDelete.ForEach(x => UnitOfWork.GetRepository<RolePermission>().HardDelete(x));
            
            newPermissions.ForEach(x => role.RolePermissions.Add(x));

            await UnitOfWork.CommitAsync();

            return Mapper.Map<RoleDto>(role);
        }
    }

public sealed class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleUpdateCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.RoleUpdateDto.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MustAsync(async (dto, value,z) => await IsNameTaken(dto))
            .WithMessage(x => $"Name {x.RoleUpdateDto.Name} is taken");
    }

    private async Task<bool> IsNameTaken(RoleUpdateCommand roleCommand)
    {
        var role = await _unitOfWork.GetRepository<Role>().AsTableNoTracking
            .FirstOrDefaultAsync(x => x.Name.ToLower() == roleCommand.RoleUpdateDto.Name.ToLower().Replace(" ", ""));

        return role == null || role.Id == roleCommand.RoleUpdateDto.Id;
    }
}
