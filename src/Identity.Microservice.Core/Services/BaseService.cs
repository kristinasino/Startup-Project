using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Microservice.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Core.Interfaces;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Services
{
    public class BaseService : IBaseService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly IHttpContextAccessor ContextAccessor;
        
        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        } 
        
        public BaseService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            UnitOfWork = unitOfWork;
            ContextAccessor = httpContextAccessor;
        }
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }
        
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            ContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetCurrentUser()
        {
           var userId = ContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

           if (userId == null)
               throw new UnauthorizedAccessException();

           var user = await UnitOfWork.GetRepository<User>().AsTrackingEntity
               .Include(x => x.UserRoles)
               .ThenInclude(x => x.Role)
               .ThenInclude(x => x.RolePermissions)
               .ThenInclude(x => x.Permission)
               .FirstOrDefaultAsync(x => x.Id == int.Parse(userId));

           if (user == null)
               throw new UnauthorizedAccessException();

           return user;
        }

    }
}