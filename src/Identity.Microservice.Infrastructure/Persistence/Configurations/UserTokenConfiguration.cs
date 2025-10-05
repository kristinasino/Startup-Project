using Identity.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class UserTokenConfiguration : BaseEntityConfiguration<UserToken>
{
    public override void PostConfigure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey("Id");
    }
}