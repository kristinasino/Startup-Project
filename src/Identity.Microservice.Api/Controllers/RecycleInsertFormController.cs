using Identity.Microservice.Core.Commands;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Dto.FilterPagination;
using Identity.Microservice.Core.Queries.RecycleInsertForms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shared.Common.Extensions;
using Shared.Entities.Enums;

namespace UserModule.Web.Controllers;

[AllowAnonymous]
[Route("api/recycle-insert-form")]
public class RecycleInsertFormController : BaseApiController
{
    [HttpGet]
    [ClaimRequirement(PermissionEnum.RecycleFormView)]
    public async Task<PaginateDto<RecycleInsertFormListDto>> List([FromQuery]RecycleInsertFormFilterDto query)
    {
        return await Mediator.Send(new RecycleInsertFormListQuery(query));
    }

    [HttpGet]
    [Route("{id}")]
    [ClaimRequirement(PermissionEnum.RecycleFormView)]
    public async Task<RecycleInsertFormDto> GetById(int id)
    {
        return await Mediator.Send(new RecycleInsertFormGetByIdQuery(id));
    }
    
    [HttpGet]
    [Route("yearsHistory")]
    [ClaimRequirement(PermissionEnum.RecycleFormView)]
    public async Task<int[]> GetYearsOfForms()
    {
        return await Mediator.Send(new RecycleInsertFormYearsHistoryQuery());
    }

    [HttpPost]
    [ClaimRequirement(PermissionEnum.RecycleFormCreate)]
    public async Task<RecycleInsertFormListDto> Create(RecycleInsertFormCreateOrUpdateDto command)
    {
        return await Mediator.Send(new RecycleInsertFormCreateCommand(command));
    }

    [HttpPut]
    [Route("{id}")]
    [ClaimRequirement(PermissionEnum.RecycleFormUpdate)]
    public async Task<ActionResult<RecycleInsertFormListDto>> Update(int id, [FromBody] RecycleInsertFormCreateOrUpdateDto command)
    {
        command.Id = id;

        return await Mediator.Send(new RecycleInsertFormUpdateCommand(command));
    }

    [HttpDelete]
    [Route("{id}")]
    [ClaimRequirement(PermissionEnum.RecycleFormUpdate)]
    public async Task Delete(int id)
    {
        await Mediator.Send(new RecycleInsertFormDeleteCommand(id));
    }
}