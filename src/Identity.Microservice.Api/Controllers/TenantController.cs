using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Microservice.Core.Commands.Tenants.Create;
using Identity.Microservice.Core.Commands.Tenants.Delete;
using Identity.Microservice.Core.Commands.Tenants.Update;
using Identity.Microservice.Core.Dto.Tenants;
using Identity.Microservice.Core.Queries.Tenants.GetById;
using Identity.Microservice.Core.Queries.Tenants.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Exceptions;
using Shared.Common.Extensions;
using Shared.Entities.Enums;
using UserModule.Web.Controllers;

namespace Identity.Microservice.Api.Controllers;

public class TenantController : BaseApiController
{
    [HttpGet]
    public async Task<IEnumerable<TenantDto>> List()
    {
        return await Mediator.Send(new TenantListQuery());
    }
    
    [HttpGet("{id:int}")]
    public async Task<TenantDto> GetById(int id)
    {
        return await Mediator.Send(new TenantGetByIdQuery(id));
    }

    [HttpPost]
    [ClaimRequirement(PermissionEnum.TenantInsert)]
    public async Task Create(TenantCreateDto tenantCreateDto)
    {
        await Mediator.Send(new TenantCreateCommand(tenantCreateDto));
    }

    [HttpPut("{id:int}")]
    [ClaimRequirement(PermissionEnum.TenantInsert)]
    public async Task Update(int id, [FromBody] TenantDto tenantDto)
    {
        if (id != tenantDto.Id)
            throw new BadRequestException("Invalid Id");

        await Mediator.Send(new TenantUpdateCommand(tenantDto));
    }

    [HttpDelete("{id:int}")]
    [ClaimRequirement(PermissionEnum.TenantInsert)]
    public async Task Delete(int id)
    {
        await Mediator.Send(new TenantDeleteCommand(id));
    }
}