using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto.Identity;
using FluentValidation;
using Identity.Microservice.Core.Dto.Identity;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;
using Web.IdentityFactory;

namespace Identity.Microservice.Core.Commands.Users.SignUp;

public record UserSignUpCommand(SignUpDto SignUpDto) : IRequest<string>;
    

public class UserSignUpHandler : BaseService, IRequestHandler<UserSignUpCommand, string>
{
    private readonly CustomUserManager _userManager;

    public UserSignUpHandler(IUnitOfWork unitOfWork, IMapper mapper, CustomUserManager userManager) : base(unitOfWork,
        mapper)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
    {
        var signUpDto = request.SignUpDto;
        var mappedUser = Mapper.Map<Role>(signUpDto);
            
        // mappedUser.UserRoles.Add(new UserRole() { RoleId = 1 });
            
       // var password = _userManager.PasswordHasher.HashPassword(mappedUser, signUpDto.Password);
            
        //mappedUser.PasswordHash = password;
            
        // var newUser = await UnitOfWork.GetRepository<User>()
        //     .AddAsync(mappedUser);

        await UnitOfWork.CommitAsync();

        var token = "await _userManager.GenerateEmailConfirmationTokenAsync()";

        return token;
    }
}

public sealed class UserSignUpCommandValidator : AbstractValidator<UserSignUpCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserSignUpCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.SignUpDto.FirstName)
            .MinimumLength(2);
        RuleFor(x => x.SignUpDto.LastName)
            .MinimumLength(2);
        RuleFor(x => x.SignUpDto.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (x, y) => await IsEmailTaken(x))
            .WithMessage("Email must be unique");
        RuleFor(x => x.SignUpDto.UserName)
            .NotEmpty()
            .MinimumLength(4)
            .MustAsync(async (x, y) => await IsUsernameTaken(x))
            .WithMessage("UserName must be unique");
        RuleFor(x => x.SignUpDto.Age)
            .NotNull()
            .WithMessage("Age is required")
            .GreaterThan(17)
            .WithMessage("Minimum age allowed is 18");
        RuleFor(x => x.SignUpDto.Gender)
            .NotNull()
            .WithMessage("Gender is required");
        RuleFor(x => x.SignUpDto.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
        RuleFor(x => x.SignUpDto.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.SignUpDto.Password)
            .WithMessage("Confirm password should be equal to password");
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




