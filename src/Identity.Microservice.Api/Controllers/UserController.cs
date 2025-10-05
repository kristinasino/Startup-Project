using System.Threading.Tasks;
using Identity.Microservice.Core.Commands.Users.Create;
using Identity.Microservice.Core.Commands.Users.SignUp;
using Identity.Microservice.Core.Commands.Users.Update;
using Identity.Microservice.Core.Dto.FilterPagination;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Core.Queries.Users.GetById;
using Identity.Microservice.Core.Queries.Users.List;
using Identity.Microservice.Core.Queries.Users.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Extensions;
using Shared.Entities.Enums;
using Shared.Entities.Models.Email;
using UserModule.Core.Commands.Users.ChangeStatus;
using UserModule.Core.Commands.Users.Delete;

namespace UserModule.Web.Controllers
{
    public class UserController : BaseApiController
    {
       
        [HttpGet("search")]
        [ClaimRequirement(PermissionEnum.UserView)]
        public async Task<PaginateDto<UserDto>> List([FromQuery] UserFilterDto userFilterDto) => await Mediator.Send(new UserListQuery(userFilterDto));
        

        [HttpGet("{id:int}")]
        [ClaimRequirement(PermissionEnum.UserView)]
        public async Task<UserDto> GetById(int id) => await Mediator.Send(new UserGetByIdQuery(id));
        
        [HttpGet("profile")]
        public async Task<UserDto> Profile(int id) => await Mediator.Send(new UserProfileQuery(id));
        
        [HttpPost]
        [ClaimRequirement(PermissionEnum.UserCreate)]
        public async Task<UserDto> Create(UserCreateDto userCreateDto) => await Mediator.Send(new UserCreateCommand(userCreateDto));


        [HttpPut("{id:int}")]
        [ClaimRequirement(PermissionEnum.UserEdit)]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (id != userUpdateDto.Id)
                return BadRequest();
            
            return await Mediator.Send(new UserUpdateCommand(userUpdateDto));
        }
        
        // [AllowAnonymous]
        // [HttpPost("sign-up")]
        // public async Task SignUp(UserSignUpCommand userSignUpCommand)
        // {
        //     var token = await Mediator.Send(userSignUpCommand);
        //
        //     var emailRequest = new EmailRequestDto();
        //     var url = "http://front-end-url?token=" + token;
        //
        //     emailRequest.Subject = "Account Verification";
        //     emailRequest.Message = "Click link below to complete account verification: <br> <br> <a href=" + url +
        //                            ">Click here</a>";
        //     emailRequest.Recipents = new string[] { userSignUpCommand.SignUpDto.Email };
        // }
        
        [HttpPut("{id:int}/change-status")]
        [ClaimRequirement(PermissionEnum.UserEdit)]
        public async Task ChangeStatus(int id)
        {
            await Mediator.Send(new UserChangeStatusCommand(id));
        }
        
        [HttpDelete("{id:int}")]
        [ClaimRequirement(PermissionEnum.UserDelete)]
        public async Task Delete(int id)
        {
            await Mediator.Send(new UserDeleteCommand(id));
        }
    }
}