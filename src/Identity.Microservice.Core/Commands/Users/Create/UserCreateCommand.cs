using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Email.Microservice.Core.Configuration;
using FluentValidation;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Core.Interfaces;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PasswordGenerator;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Commands.Users.Create;

public record UserCreateCommand(UserCreateDto UserCreateDto) : IRequest<UserDto>;

public class UserCreateHandler : BaseService, IRequestHandler<UserCreateCommand, UserDto>
{
    private readonly CustomUserManager _userManager;
    private readonly IEmailService _emailService;
    private readonly EmailSettings _emailSettings;

    public UserCreateHandler(IUnitOfWork unitOfWork, IMapper mapper, CustomUserManager userManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor, IOptions<EmailSettings> mailSettings) : base(unitOfWork,
        mapper, httpContextAccessor)
    {
        _userManager = userManager;
        _emailService = emailService;
        _emailSettings = mailSettings.Value;
    }

    public async Task<UserDto> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var mappedUser = Mapper.Map<User>(request.UserCreateDto);

        var currentUser = await GetCurrentUser();

        if (request.UserCreateDto.TenantId.HasValue)
        {
            var tenantExists = await UnitOfWork.GetRepository<Tenant>().AsTableNoTracking
                .AnyAsync(x => x.Id == request.UserCreateDto.TenantId, cancellationToken);

            if (!tenantExists)
                throw new BadRequestException("Requested tenant does not exist. Try with another tenant!");   
        }

        mappedUser.UserRoles.Add(new UserRole() { RoleId = request.UserCreateDto.RoleId });

        var randomPassword = new Password(true, true, true, true, 8).Next();

        var password = _userManager.PasswordHasher.HashPassword(mappedUser, randomPassword);

        mappedUser.PasswordHash = password;

        var transaction = await UnitOfWork.BeginTransactionAsync();

        if (!request.UserCreateDto.TenantId.HasValue)
            mappedUser.TenantId = currentUser.TenantId;
        
        var newUser = await UnitOfWork.GetRepository<User>()
            .AddAsync(mappedUser);

        await UnitOfWork.CommitAsync();

        var url = _emailSettings.WebsiteUrl;

        var emailBody = $"Hello {newUser.Entity.FirstName}, <br> <br> Welcome to WRMS! <br> <br> The password to login is: <strong> {randomPassword} </strong> " +
                        $"<br> <br> Click link below to get redirected to our website: <br> <br> <a href=" + url +
                        ">Click here</a>";

        await _emailService.SendEmailAsync(new []{ newUser.Entity.Email }, "Account Activation Email", emailBody);

        newUser.Entity.EmailSent = true;

        await UnitOfWork.CommitAsync();
        
        await transaction.CommitAsync(cancellationToken);

        return Mapper.Map<UserDto>(newUser.Entity);
    }
}

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserCreateCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.UserCreateDto.FirstName)
            .MinimumLength(2);
        RuleFor(x => x.UserCreateDto.LastName)
            .MinimumLength(2);
        RuleFor(x => x.UserCreateDto.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (x, y) => await IsEmailTaken(x))
            .WithMessage("Email must be unique");
        RuleFor(x => x.UserCreateDto.UserName)
            .NotEmpty()
            .MinimumLength(4)
            .MustAsync(async (x, y) => await IsUsernameTaken(x))
            .WithMessage("UserName must be unique");
      
        RuleFor(x => x.UserCreateDto.Gender)
            .NotNull()
            .WithMessage("Gender is required");
    }

    private async Task<bool> IsEmailTaken(string email)
    {
        var user = await _unitOfWork.GetRepository<User>().AsTableNoTracking
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        return user == null;
    }

    private async Task<bool> IsUsernameTaken(string userName)
    {
        var user = await _unitOfWork.GetRepository<User>().AsTableNoTracking
            .FirstOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower().Replace(" ", ""));

        return user == null;
    }
}