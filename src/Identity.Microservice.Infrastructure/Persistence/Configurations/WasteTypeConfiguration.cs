using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class WasteTypeConfiguration : BaseEntityConfiguration<WasteType>
{
    public override void PostConfigure(EntityTypeBuilder<WasteType> builder)
    {
        builder.HasKey("Id");
    }
}