using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto.Role;
using Identity.Microservice.Core.Commands.Roles.Create;
using Identity.Microservice.Core.Dto.Role;
using Identity.Microservice.Core.Queries.Roles.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Extensions;
using Shared.Entities.Enums;
using UserModule.Core.Commands.Roles.Update;
using UserModule.Core.Queries.Roles.GetById;

namespace UserModule.Web.Controllers
{
    [Route("api/role")]
    public class RoleController : BaseApiController
    {
        
        [HttpGet]
       //[ClaimRequirement(PermissionEnum.RoleView)]
        public async Task<IEnumerable<RoleDto>> List()
        {
            return await Mediator.Send(new RoleListQuery());
        }

        [HttpGet("{id:int}")]
        [ClaimRequirement(PermissionEnum.RoleView)]
        public async Task<RoleDto> GetById(int id)
        {
            return await Mediator.Send(new RoleGetByIdQuery(id));
        }

        [HttpPost]
        [ClaimRequirement(PermissionEnum.RoleCreate)]
        public async Task<RoleDto> Create(RoleCreateDto roleCreateDto)
        {
            return await Mediator.Send(new RoleCreateCommand(roleCreateDto));
        }
        
        [HttpPut("{id:int}")]
        [ClaimRequirement(PermissionEnum.RoleEdit)]
        public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] RoleUpdateDto roleUpdateDto)
        {
            if (id != roleUpdateDto.Id)
                return BadRequest();
            
            return await Mediator.Send(new RoleUpdateCommand(roleUpdateDto));
        }
    }
}