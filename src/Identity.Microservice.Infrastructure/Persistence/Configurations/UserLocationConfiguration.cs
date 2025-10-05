using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class UserLocationConfiguration : BaseEntityConfiguration<UserLocation>
{
    public override void PostConfigure(EntityTypeBuilder<UserLocation> builder)
    {
        builder.HasKey("Id");
    }
}