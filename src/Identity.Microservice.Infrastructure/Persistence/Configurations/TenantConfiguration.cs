using Domain.Entities;
using Identity.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : BaseEntityConfiguration<Tenant>
{
    public override void PostConfigure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey("Id");
    }
}