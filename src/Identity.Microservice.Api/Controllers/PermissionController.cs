using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Identity.Microservice.Core.Queries.Permissions.List;
using Microsoft.AspNetCore.Mvc;
using Web.IdentityFactory;

namespace UserModule.Web.Controllers
{
    [Route("api/permission")]
    public class PermissionController : BaseApiController
    {

        [HttpGet]
        public async Task<IEnumerable<PermissionDto>> List()
        {
            return await Mediator.Send(new PermissionsListQuery());
        }
    }
}