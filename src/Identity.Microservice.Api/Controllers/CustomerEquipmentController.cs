using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Microservice.Core.Commands.CustomerEquipment.Create;
using Identity.Microservice.Core.Commands.CustomerEquipment.Delete;
using Identity.Microservice.Core.Commands.CustomerEquipment.Update;
using Identity.Microservice.Core.Dto.CustomerEquipment;
using Identity.Microservice.Core.Queries.CustomerEquipment.GetById;
using Identity.Microservice.Core.Queries.CustomerEquipment.List;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Exceptions;
using Shared.Common.Extensions;
using Shared.Entities.Enums;
using UserModule.Web.Controllers;

namespace Identity.Microservice.Api.Controllers;

[ClaimRequirement(PermissionEnum.CustomerLocationEquipmentInsert)]
public class CustomerEquipmentController : BaseApiController
{
    [HttpGet]
    public async Task<IEnumerable<CustomerEquipmentDto>> List()
    {
        return await Mediator.Send(new CustomerEquipmentListQuery());
    }

    [HttpGet("{id:int}")]
    public async Task<CustomerEquipmentDto> GetById(int id)
    {
        return await Mediator.Send(new CustomerEquipmentGetByIdQuery(id));
    }

    [HttpPost]
    public async Task Create(CustomerEquipmentCreateDto customerEquipmentCreateDto)
    {
        await Mediator.Send(new CustomerEquipmentCreateCommand(customerEquipmentCreateDto));
    }

    [HttpPut("{id:int}")]
    public async Task Update(int id, [FromBody] CustomerEquipmentUpdateDto contractUpdateDto)
    {
        if (id != contractUpdateDto.Id)
            throw new BadRequestException("Invalid Id");

        await Mediator.Send(new CustomerEquipmentUpdateCommand(contractUpdateDto));
    }

    [HttpDelete("{id:int}")]
    public async Task Delete(int id)
    {
        await Mediator.Send(new CustomerEquipmentDeleteCommand(id));
    }
}