using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class EquipmentTypeConfiguration : BaseEntityConfiguration<EquipmentType>
{
    public override void PostConfigure(EntityTypeBuilder<EquipmentType> builder)
    {
        builder.HasKey("Id");
    }
}