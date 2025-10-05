using System.Threading.Tasks;
using Identity.Microservice.Domain.Entities;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Repository;

public interface IUserRepository
{
    Task<User> GetByUserName(string userName);
    Task<User> GetByEmail(string email);
}