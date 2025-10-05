using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Microservice.Core.Commands.Location.Create;
using Identity.Microservice.Core.Commands.Location.Delete;
using Identity.Microservice.Core.Commands.Location.Update;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Queries.Locations.GetById;
using Identity.Microservice.Core.Queries.Locations.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Exceptions;
using Shared.Common.Extensions;
using Shared.Entities.Enums;

namespace UserModule.Web.Controllers;

public class LocationController : BaseApiController
{
    [HttpGet]
    public async Task<IEnumerable<LocationDto>> List([FromQuery] LocationListQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id:int}")]
    public async Task<LocationDto> GetById(int id)
    {
        return await Mediator.Send(new LocationGetByIdQuery(id));
    }
    
    [HttpGet("search")]
    public async Task<IEnumerable<LocationDto>> Search([FromQuery] string searchTerm)
    {
        return await Mediator.Send(new LocationSearchQuery(searchTerm));
    }

    [HttpPost]
    [ClaimRequirement(PermissionEnum.LocationInsert)]
    public async Task Create(LocationCreateDto locationCreateDto)
    {
         await Mediator.Send(new CustomerLocationCreateCommand(locationCreateDto));
    }

    [HttpPut("{id:int}")]
    [ClaimRequirement(PermissionEnum.LocationInsert)]
    public async Task Update(int id, [FromBody] LocationUpdateDto locationUpdateDto)
    {
        if (id != locationUpdateDto.Id)
            throw new BadRequestException("Invalid Id");

        await Mediator.Send(new CustomerLocationUpdateCommand(locationUpdateDto));
    }

    [HttpDelete("{id:int}")]
    [ClaimRequirement(PermissionEnum.LocationInsert)]
    public async Task Delete(int id)
    {
        await Mediator.Send(new CustomerLocationDeleteCommand(id));
    }
}