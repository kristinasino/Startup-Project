using System.Threading.Tasks;
using Identity.Microservice.Core.Repository;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Infrastructure.Repository;
using UserModule.Core.Repository;

namespace Shared.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();
        /// <returns>The number of objects in an Added, Modified, or Deleted state asynchronously</returns>
        Task<int> CommitAsync();
        /// <returns>Repository</returns>
        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        Task<IDbContextTransaction> BeginTransactionAsync();
        
        public IUserRepository User {get;}
    }
}