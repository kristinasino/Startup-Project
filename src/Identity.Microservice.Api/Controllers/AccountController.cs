using System.Threading.Tasks;
using Core.Dto;
using Core.Dto.Identity;
using Identity.Microservice.Core.Commands.Auth.ForgotPassword;
using Identity.Microservice.Core.Commands.Auth.Login;
using Identity.Microservice.Core.Dto.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserModule.Core.Commands.Auth.PasswordReset;
using UserModule.Core.Dto.Identity;
using UserModule.Web.Controllers;
using Web.IdentityFactory;

namespace Identity.Microservice.Api.Controllers
{
    [Route("api/account")]
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<AuthenticateResponseDto> Login(LoginDto loginDto)
        {
            return await Mediator.Send(new LoginCommand(loginDto));
        }

        [HttpPost("forgot-password")]
        public async Task ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto) => await Mediator.Send(new ForgotPasswordCommand(forgotPasswordDto));
        
        
        [HttpPost("password-reset")]
        public async Task PasswordReset(PasswordResetDto passwordResetDto)
        {
            await Mediator.Send(new PasswordResetCommand(passwordResetDto));
        }
        
        [HttpPost("change-password")]
        [Authorize]
        public async Task PasswordReset(ChangePasswordDto changePasswordDto)
        {
            await Mediator.Send(new ChangePasswordCommand(changePasswordDto));
        }
        
        // [HttpPost("email-confirmation")]
        // public async Task EmailConfirmation(ConfirmEmailDto confirmEmailDto)
        // {
        //     await _authService.ConfirmEmail(confirmEmailDto);
        // }
        
        [HttpPost("refresh-token")]
        public async Task<AuthenticateResponseDto> RefreshToken(RequestRefreshTokenDto requestRefreshTokenDto)
        {
           return await _tokenService.RefreshToken(requestRefreshTokenDto);
        }  
         
        [HttpDelete("revoke-token")]
        public async Task RevokeToken(string refreshToken)
        { 
            await _tokenService.RemoveRefreshTokenAsync(refreshToken);
        }

    }
}