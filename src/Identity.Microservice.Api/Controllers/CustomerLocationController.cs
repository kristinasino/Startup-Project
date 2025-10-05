using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Identity.Microservice.Core.Dto.CustomerLocation;
using Identity.Microservice.Core.Queries.CustomerLocations.List;
using Microsoft.AspNetCore.Mvc;
using UserModule.Web.Controllers;

namespace Identity.Microservice.Api.Controllers;

public class CustomerLocationController : BaseApiController
{
    [HttpGet]
    public async Task<IEnumerable<CustomerLocationDto>> List([FromQuery] CustomerLocationsListQuery customerLocationsListQuery)
    {
        return await Mediator.Send(customerLocationsListQuery);
    }
}