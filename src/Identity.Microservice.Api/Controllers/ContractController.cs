using Identity.Microservice.Core.Commands.Contract.Create;
using Identity.Microservice.Core.Commands.Contract.Delete;
using Identity.Microservice.Core.Commands.Contract.Update;
using Identity.Microservice.Core.Dto.Contract;
using Identity.Microservice.Core.Queries.Contract.GetById;
using Identity.Microservice.Core.Queries.Contract.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Common.Extensions;
using Shared.Entities.Enums;
using UserModule.Web.Controllers;

namespace Identity.Microservice.Api.Controllers
{
    [ClaimRequirement(PermissionEnum.CustomerContractInsert)]
    public class ContractController : BaseApiController
    {
        [HttpGet]
        public async Task<IEnumerable<ContractDto>> List()
        {
            return await Mediator.Send(new ContractListQuery());
        }

        [HttpGet("{id:int}")]
        public async Task<ContractDto> GetById(int id)
        {
            return await Mediator.Send(new ContractGetByIdQuery(id));
        }

        [HttpPost]
        public async Task Create(ContractCreateDto contractCreateDto)
        {
            await Mediator.Send(new CustomerContractCreateCommand(contractCreateDto));
        }

        [HttpPut("{id:int}")]
        public async Task Update(int id, [FromBody] ContractUpdateDto contractUpdateDto)
        {
            if (id != contractUpdateDto.Id)
                throw new BadRequestException("Invalid Id");

            await Mediator.Send(new CustomerContractUpdateCommand(contractUpdateDto));
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new CustomerContractDeleteCommand(id));
        }
    }
}
