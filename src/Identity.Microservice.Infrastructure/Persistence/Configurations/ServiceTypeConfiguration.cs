using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class ServiceTypeConfiguration : BaseEntityConfiguration<ServiceType>
{
    public override void PostConfigure(EntityTypeBuilder<ServiceType> builder)
    {
        builder.HasKey("Id");
    }
}