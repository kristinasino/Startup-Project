using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;
using Web.IdentityFactory;

namespace Identity.Microservice.Core.Commands.Users.Update;


    public record UserUpdateCommand(UserUpdateDto UserUpdateDto) : IRequest<UserDto>;
    
    
    public class UserUpdateHandler : BaseService, IRequestHandler<UserUpdateCommand, UserDto>
    {
        private readonly CustomUserManager _userManager;

        public UserUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper, CustomUserManager userManager) : base(unitOfWork,
            mapper)
        {
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var newLocations = new List<UserLocation>();
            var userUpdateDto = request.UserUpdateDto;
            var user = await UnitOfWork.GetRepository<User>()
                .AsTrackingEntity
                .Include(x => x.UserRoles)
                .Include(x => x.UserLocations)
                .FirstOrDefaultAsync(x => x.Id == userUpdateDto.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException("");
            
            if (request.UserUpdateDto.TenantId.HasValue)
            {
                var tenantExists = await UnitOfWork.GetRepository<Tenant>().AsTableNoTracking
                    .AnyAsync(x => x.Id == request.UserUpdateDto.TenantId, cancellationToken);

                if (!tenantExists)
                    throw new BadRequestException("Requested tenant does not exist. Try with another tenant!");   
            }

            Mapper.Map(userUpdateDto, user);

            if (user.UserRoles.Any(x => x.RoleId != userUpdateDto.RoleId))
            {
                UnitOfWork.GetRepository<UserRole>().HardDelete(user.UserRoles.First()); 
                user.UserRoles.Add(new UserRole() {RoleId = userUpdateDto.RoleId});
            }
            
            var currentLocations = user.UserLocations;

            if (userUpdateDto.Locations is {Length: > 0})
            {
                newLocations = userUpdateDto.Locations
                    .Where(x => !currentLocations.Select(y => y.LocationId).Contains(x))
                    .Select(x => new UserLocation()
                    {
                        UserId = user.Id,
                        LocationId = x
                    }).ToList();
            }

            var locationsToDelete = currentLocations
                .Where(x => !userUpdateDto.Locations.Contains(x.LocationId)).ToList();
            
            locationsToDelete.ForEach(x => UnitOfWork.GetRepository<UserLocation>().SoftDelete(x));
            
            newLocations.ForEach(x => user.UserLocations.Add(x));

            await UnitOfWork.CommitAsync();

            return Mapper.Map<UserDto>(user);
        }
    }

public sealed class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserUpdateCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.UserUpdateDto.FirstName)
            .MinimumLength(2);
        RuleFor(x => x.UserUpdateDto.LastName)
            .MinimumLength(2);
    }
}
