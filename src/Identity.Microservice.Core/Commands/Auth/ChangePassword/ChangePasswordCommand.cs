using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Core.Dto.Identity;
using Identity.Microservice.Core.Dto.Identity;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using Web.IdentityFactory;

namespace UserModule.Core.Commands.Auth.PasswordReset;

public record ChangePasswordCommand(ChangePasswordDto ChangePasswordDto) : IRequest;

public class ChangePasswordHandler : BaseService, IRequestHandler<ChangePasswordCommand>
{
    private readonly CustomUserManager _customUserManager;
    public ChangePasswordHandler(IUnitOfWork unitOfWork, CustomUserManager customUserManager, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, httpContextAccessor)
    {
        _customUserManager = customUserManager;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var passwordResetDto = request.ChangePasswordDto;
        var user = await GetCurrentUser();

        if (user == null)
            throw new BadRequestException("User is not found!");

        var result = await _customUserManager.CheckPasswordAsync(user, passwordResetDto.oldPassword);

        if (!result)
            throw new BadRequestException("Invalid old password!");
        
        var hashedPassword = _customUserManager.PasswordHasher.HashPassword(user, passwordResetDto.newPassword);

        user.PasswordHash = hashedPassword;

        await UnitOfWork.CommitAsync();
    }
}