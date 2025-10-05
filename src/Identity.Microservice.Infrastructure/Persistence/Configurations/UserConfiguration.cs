using Identity.Microservice.Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void PostConfigure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Gender)
                .HasConversion<string>();
        }
    }
}