using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.Microservice.Core.Repository;
using Identity.Microservice.Domain.Entities;
using Infrastructure.DAL;
using Infrastructure.DAL.Factory;
using Microsoft.EntityFrameworkCore;
using UserModule.Core.Repository;
using UserModule.Domain.Entities;

namespace Shared.Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IContextFactory contextFactory) : base(contextFactory.DbContext)
    { }

    public async Task<User> GetByUserName(string userName)
    {
        return await _dbSet
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .ThenInclude(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
    }
    
    public async Task<User> GetByEmail(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }
}