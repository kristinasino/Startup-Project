using Identity.Microservice.Core.Dto.Common;
using Identity.Microservice.Core.Dto.Lookup;
using Identity.Microservice.Core.Queries.Lookup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserModule.Web.Controllers;

[AllowAnonymous]
[Route("api/lookup")]
public class LookupController : BaseApiController
{
    [HttpGet]
    [Route("waste-type")]
    public async Task<IEnumerable<LookupBaseDto>> ListWasteType()
    {
        return await Mediator.Send(new WasteTypeListQuery());
    }


    [HttpGet]
    [Route("method-type")]
    public async Task<IEnumerable<LookupBaseDto>> ListMethodType()
    {
        return await Mediator.Send(new MethodTypeListQuery());
    }

    [HttpGet]
    [Route("material-type")]
    public async Task<IEnumerable<LookupBaseDto>> ListMaterialType()
    {
        return await Mediator.Send(new MaterialTypeListQuery());
    }

    [HttpGet]
    [Route("service-type")]
    public async Task<IEnumerable<LookupBaseDto>> ListServiceType()
    {
        return await Mediator.Send(new ServiceTypeListQuery());
    }

    [HttpGet]
    [Route("contract-type")]
    public async Task<IEnumerable<LookupBaseDto>> ListContractType()
    {
        return await Mediator.Send(new ContractTypeListQuery());
    }

    [HttpGet]
    [Route("equipment-type")]
    public async Task<IEnumerable<EquipmentTypeDto>> ListEquipmentType()
    {
        return await Mediator.Send(new EquipmentTypeListQuery());
    }
}