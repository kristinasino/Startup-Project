using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Queries.Users.GetById;


    public record UserGetByIdQuery(int Id) : IRequest<UserDto>;
    
    
    public class UserGetByIdHandler : BaseService, IRequestHandler<UserGetByIdQuery, UserDto>
    {
        public UserGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<UserDto> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.GetRepository<User>().AsTableNoTracking
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Include(x => x.UserLocations)
                .ThenInclude(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (user == null)
                throw new NotFoundException("");
            
            return Mapper.Map<UserDto>(user);
        }
    }
