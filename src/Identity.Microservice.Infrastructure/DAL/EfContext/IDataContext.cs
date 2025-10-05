using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DAL
{
    public interface IDataContext
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        /// <returns>number of state entries interacted with database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <returns>number of state entries interacted with database</returns>
        Task<int> SaveChangesAsync(bool confirmAllTransactions, CancellationToken cancellationToken);

        /// <returns>number of state entries interacted with database</returns>
        int SaveChanges();

        /// <returns>number of state entries interacted with database</returns>
        int SaveChanges(bool confirmAllTransactions);

        Task<IDbContextTransaction> BeginTransaction();

        void Dispose();
    }
}