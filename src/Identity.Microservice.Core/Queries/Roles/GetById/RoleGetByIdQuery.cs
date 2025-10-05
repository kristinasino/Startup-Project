using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto.Role;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace UserModule.Core.Queries.Roles.GetById;


    public record RoleGetByIdQuery(int Id) : IRequest<RoleDto>;
    
    
    public class RoleGetByIdHandler : BaseService, IRequestHandler<RoleGetByIdQuery, RoleDto>
    {
        public RoleGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<RoleDto> Handle(RoleGetByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await UnitOfWork.GetRepository<Role>().AsTableNoTracking
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (role == null)
                throw new NotFoundException("");
            
            return Mapper.Map<RoleDto>(role);
        }
    }
