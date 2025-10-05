using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities.Constants;
using Shared.Entities.Enums;

namespace Shared.Common.Extensions
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(PermissionEnum claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(GlobalConstants.AppAccess, claimValue.ToString()) };
        }
    }
}