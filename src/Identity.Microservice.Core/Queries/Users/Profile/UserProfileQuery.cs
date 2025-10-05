using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Core.Queries.Users.GetById;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.Users.Profile;


    public record UserProfileQuery(int Id) : IRequest<UserDto>;
    
    
    public class UserProfileHandler : BaseService, IRequestHandler<UserProfileQuery, UserDto>
    {
        public UserProfileHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<UserDto> Handle(UserProfileQuery request, CancellationToken cancellationToken)
        {
            var loggedInUser = await GetCurrentUser();

            return Mapper.Map<UserDto>(loggedInUser);
        }
    }
