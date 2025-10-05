using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class MaterialTypeConfiguration : BaseEntityConfiguration<MaterialType>
{
    public override void PostConfigure(EntityTypeBuilder<MaterialType> builder)
    {
        builder.HasKey("Id");
    }
}