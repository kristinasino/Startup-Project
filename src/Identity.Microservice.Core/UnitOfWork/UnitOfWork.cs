using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Core.Repository;
using Infrastructure.DAL;
using Infrastructure.DAL.Factory;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Infrastructure.Repository;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Core.Repository;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDataContext dbContext;

        private Dictionary<Type, object> repos;

        public IUserRepository User { get; }

        public UnitOfWork(IContextFactory contextFactory, IUserRepository user)
        {
            dbContext = contextFactory.DbContext;
            User = user;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            if (repos == null)
            {
                repos = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repos.ContainsKey(type))
            {
                repos[type] = new GenericRepository<TEntity>(dbContext);
            }

            return (IGenericRepository<TEntity>)repos[type];
        }

        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            return dbContext.SaveChanges();
        }
        public async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await dbContext.BeginTransaction();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(obj: this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                    dbContext = null;
                }
            }
        }
    }
}