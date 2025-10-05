using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Core.Dto.Identity;
using Identity.Microservice.Core.Dto.Identity;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using Web.IdentityFactory;

namespace UserModule.Core.Commands.Auth.PasswordReset;

public record PasswordResetCommand(PasswordResetDto PasswordResetDto) : IRequest;

public class ForgotPasswordHandler : BaseService, IRequestHandler<PasswordResetCommand>
{
    private readonly CustomUserManager _customUserManager;
    public ForgotPasswordHandler(IUnitOfWork unitOfWork, CustomUserManager customUserManager) : base(unitOfWork)
    {
        _customUserManager = customUserManager;
    }

    public async Task Handle(PasswordResetCommand request, CancellationToken cancellationToken)
    {
        var passwordResetDto = request.PasswordResetDto;
        var user = await UnitOfWork.GetRepository<User>().AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Email.ToLower() == passwordResetDto.Email.ToLower());

        if (user == null)
            throw new BadRequestException("User is not found!");

        var result = await _customUserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider,UserManager<User>.ResetPasswordTokenPurpose, passwordResetDto.Token);

        if (!result)
            throw new BadRequestException("Invalid token!");
        
        var hashedPassword = _customUserManager.PasswordHasher.HashPassword(user, passwordResetDto.Password);

        user.PasswordHash = hashedPassword;

        await UnitOfWork.CommitAsync();
    }
}