using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : BaseEntityConfiguration<CustomerLocation>
{
    public override void PostConfigure(EntityTypeBuilder<CustomerLocation> builder)
    {
        builder.HasKey("Id");
    }
}