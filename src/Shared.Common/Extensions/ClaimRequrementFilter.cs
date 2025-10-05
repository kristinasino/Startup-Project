using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.Common.Extensions
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.User.Claims;

            var hasClaim = claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
             if (!hasClaim)
                 context.Result = new ForbidResult();
        }
    }
}