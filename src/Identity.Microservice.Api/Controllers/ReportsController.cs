using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Extensions;
using Shared.Entities.Enums;
using UserModule.Web.Controllers;
using Web.IdentityFactory;

namespace Identity.Microservice.Api.Controllers
{
    public class ReportsController : BaseApiController
    {
        private readonly ITokenService _tokenService;

        public ReportsController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        //[ClaimRequirement(PermissionEnum.RecycleFormCreate)]
        public async Task<IActionResult> Report()
        {
            var userId = GetUserId;

            if (userId == 0)
                return Unauthorized();
            
            var token = await _tokenService.GenerateReportsToken(userId);
        
            return Ok(new {token});
        }
    }
}