using Identity.Microservice.Core.Commands;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Dto.FilterPagination;
using Identity.Microservice.Core.Queries.TonageInsertForms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Microservice.Core.Queries.RecycleInsertForms;
using Shared.Common.Extensions;
using Shared.Entities.Enums;

namespace UserModule.Web.Controllers;

[AllowAnonymous]
[Route("api/tonnage-insert-form")]
public class TonageInsertFormController : BaseApiController
{
    [HttpGet]
    [ClaimRequirement(PermissionEnum.TonnageFormView)]
    public async Task<PaginateDto<TonageInsertFormListDto>> List([FromQuery] TonageInsertFormFilterDto query)
    {
        return await Mediator.Send(new TonageInsertFormListQuery(query));
    }

    [HttpGet]
    [Route("{id}")]
    [ClaimRequirement(PermissionEnum.TonnageFormView)]
    public async Task<TonageInsertFormDto> GetById(int id)
    {
        return await Mediator.Send(new TonageInsertFormGetByIdQuery(id));
    }
    
    [HttpGet]
    [Route("yearsHistory")]
    [ClaimRequirement(PermissionEnum.TonnageFormView)]
    public async Task<int[]> GetYearsOfForms()
    {
        return await Mediator.Send(new TonageInsertFormYearsHistoryQuery());
    }

    [HttpPost]
    [ClaimRequirement(PermissionEnum.TonnageFormCreate)]
    public async Task<TonageInsertFormListDto> Create(TonageInsertFormCreateOrUpdateDto command)
    {
        return await Mediator.Send(new TonageInsertFormCreateCommand(command));
    }

    [HttpPut]
    [Route("{id}")]
    [ClaimRequirement(PermissionEnum.TonnageFormUpdate)]
    public async Task<ActionResult<TonageInsertFormListDto>> Update(int id, [FromBody] TonageInsertFormCreateOrUpdateDto command)
    {
        command.Id = id;

        return await Mediator.Send(new TonageInsertFormUpdateCommand(command));
    }

    [HttpDelete]
    [Route("{id}")]
    [ClaimRequirement(PermissionEnum.TonnageFormUpdate)]
    public async Task Delete(int id)
    {
        await Mediator.Send(new TonageInsertFormDeleteCommand(id));
    }
}