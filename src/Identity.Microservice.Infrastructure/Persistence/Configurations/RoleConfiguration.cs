using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public override void PostConfigure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}