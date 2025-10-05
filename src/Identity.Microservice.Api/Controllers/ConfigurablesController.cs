using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Microservice.Core.Dto.Configurables;
using Identity.Microservice.Core.Queries.Configurables;
using Microsoft.AspNetCore.Mvc;
using UserModule.Web.Controllers;

namespace Identity.Microservice.Api.Controllers;

public class ConfigurablesController : BaseApiController
{
    [HttpGet("service-type")]
    public async Task<IEnumerable<ServiceTypeDto>> ServiceTypeList()
    {
        return await Mediator.Send(new ServiceTypeListQuery());
    }
    
    [HttpGet("equipment-type")]
    public async Task<IEnumerable<EquipmentTypeDto>> EquipmentTypeList()
    {
        return await Mediator.Send(new EquipmentTypeListQuery());
    } 
    
    [HttpGet("vendor")]
    public async Task<IEnumerable<VendorDto>> VendorList()
    {
        return await Mediator.Send(new VendorTypeListQuery());
    }
    
    [HttpGet("contract-type")]
    public async Task<IEnumerable<ContractTypeDto>> ContractTypeList()
    {
        return await Mediator.Send(new ContractTypeListQuery());
    }
}