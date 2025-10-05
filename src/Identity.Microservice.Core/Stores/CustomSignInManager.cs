using System;
using Identity.Microservice.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Stores
{
    public class CustomSignInManager : SignInManager<User>
    {
        private readonly CustomUserManager _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomSignInManager(
            CustomUserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemeProvider)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }
    }
}