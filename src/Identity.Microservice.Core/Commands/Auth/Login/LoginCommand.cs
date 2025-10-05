using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Dto.User;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Core.Dto.Identity;
using UserModule.Domain.Entities;
using Web.IdentityFactory;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Identity.Microservice.Core.Commands.Auth.Login;

public record LoginCommand(LoginDto LoginDto) : IRequest<AuthenticateResponseDto>;

public class LoginHandler : BaseService, IRequestHandler<LoginCommand, AuthenticateResponseDto>
{
    private readonly CustomSignInManager _signInManager;
    private readonly ITokenService _tokenService;
    private readonly CustomUserManager _userManager;
    
    public LoginHandler(IUnitOfWork unitOfWork, CustomSignInManager signInManager, ITokenService tokenService, IMapper mapper, CustomUserManager userManager) : base(unitOfWork, mapper)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<AuthenticateResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginDto = request.LoginDto;
        var user = await UnitOfWork.User.GetByUserName(loginDto.UserName);

        if (user == null)
            throw new BadRequestException("Username or Password is incorrect!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        //manage lockout situations
        await ManageLockout(user, result);

        if (!result.Succeeded)
            throw new ArgumentException("Username or Password is incorrect!");

        if (user.AccessFailedCount > 0)
        {
            user.AccessFailedCount = 0;
            await _userManager.UpdateAsync(user);
        }

        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

        return new AuthenticateResponseDto(Mapper.Map<UserLoginDto>(user), token, refreshToken.Token);
    }
    
    #region Private Methods
    private async Task ManageLockout(User user, SignInResult result)
    {
        if (user.LockoutEnd > DateTimeOffset.Now)
            throw new ArgumentException("Your account has been locked. Contact your support person to unlock it, then try again!");
            
        if (!result.Succeeded)
        {
            if (user is not { LockoutEnabled: true })
                throw new BadRequestException("Username or Password is incorrect!");
                
            user.AccessFailedCount++;
            if (user.AccessFailedCount == 10)
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;
                user.IsActive = false;
            }
            await _userManager.UpdateAsync(user);

            throw new BadRequestException("Username or Password is incorrect!");
        }
    }
    
    #endregion
}
