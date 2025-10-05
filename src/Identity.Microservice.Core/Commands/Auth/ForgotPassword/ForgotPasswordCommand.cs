using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Dto.Identity;
using Email.Microservice.Core.Configuration;
using Identity.Microservice.Core.Interfaces;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using Web.IdentityFactory;

namespace Identity.Microservice.Core.Commands.Auth.ForgotPassword;

public record ForgotPasswordCommand(ForgotPasswordDto ForgotPasswordDto) : IRequest;

public class ForgotPasswordHandler : BaseService, IRequestHandler<ForgotPasswordCommand>
{
    private readonly CustomUserManager _customUserManager;
    private readonly EmailSettings _emailSettings;
    private readonly IEmailService _emailService;
    public ForgotPasswordHandler(IUnitOfWork unitOfWork, CustomUserManager customUserManager, IOptions<EmailSettings> emailSettings, IEmailService emailService) : base(unitOfWork)
    {
        _customUserManager = customUserManager;
        _emailService = emailService;
        _emailSettings = emailSettings.Value;
    }

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var forgetPasswordDto = request.ForgotPasswordDto;
        var user = await UnitOfWork.User.GetByEmail(forgetPasswordDto.Email);

        if (user == null)
            throw new BadRequestException("User is not found!");

        var token = await _customUserManager.GeneratePasswordResetTokenAsync(user);

        var url = $"{_emailSettings.WebsiteUrl}/auth/reset-password?token={UrlEncoder.Default.Encode(token)}&email={user.Email}";
        
        var emailBody = $"Hello {user.FirstName}, " +
                        $"<br> <br> Click link below to reset your password: <br> <br> <a href=" + url +
                        ">Click here</a>";
        await _emailService.SendEmailAsync(new []{user.Email}, "Reset Password", emailBody);
    }
}