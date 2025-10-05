using Domain.Entities.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations
{
    public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
    {
        public override void PostConfigure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}