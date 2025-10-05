using Domain.Entities.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void PostConfigure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}