using Identity.Microservice.Domain.Entities.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasQueryFilter(x => !x.IsSoftDeleted);
            builder.ToTable(typeof(T).Name);
            PostConfigure(builder);
        }


        public abstract void PostConfigure(EntityTypeBuilder<T> builder);
    }
}