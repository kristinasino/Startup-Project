using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : BaseEntityConfiguration<Vendor>
{
    public override void PostConfigure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey("Id");
    }
}