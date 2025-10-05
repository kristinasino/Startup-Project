using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Domain.Entities.BaseEntities;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Entities.Enums;
using Shared.Entities.Models.Audits;

namespace UserModule.Infrastructure.DAL.EfContext
{
    public class DataContext : DbContext, IDataContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
           
        }
        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSavingChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<IDbContextTransaction> BeginTransaction() => await Database.BeginTransactionAsync();

        private void OnBeforeSavingChanges()
        {
            var auditEntries = new List<AuditEntry>();
            var userContext = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = int.Parse(userContext is not null ? userContext.Value : "0") ;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                SetCreatedUpdated(entry, userId);
                
                //Audit part
                if (entry.State is EntityState.Detached or EntityState.Unchanged || userId == 0)
                    continue;
        
                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
        
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditEnum.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditEnum.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditEnum.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
        
            foreach (var auditEntry in auditEntries)
            {
               Set<Audit>().Add(auditEntry.ToAudit());
            }
        }

        private static void SetCreatedUpdated(EntityEntry<BaseEntity> entry, int userId)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    entry.Entity.CreatedBy = userId == 0 ? null : userId;
                    entry.Entity.CreatedAt = DateTimeOffset.Now;
                    entry.Entity.Version += 1;
                    break;
                }
                case EntityState.Modified:
                {
                    entry.Entity.UpdatedBy = userId == 0 ? null : userId;
                    entry.Entity.UpdatedAt = DateTimeOffset.Now;
                    entry.Entity.Version += 1;
                    break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Identity.Microservice.Infrastructure") 
                            || x.FullName.Contains("Shared.Infrastructure"));

            foreach (var assembly in assemblies)
            {
                builder.ApplyConfigurationsFromAssembly(assembly);
            }
            base.OnModelCreating(builder);
        }
    }
}