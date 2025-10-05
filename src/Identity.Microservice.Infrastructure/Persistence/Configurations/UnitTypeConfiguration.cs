using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class UnitTypeConfiguration : BaseEntityConfiguration<UnitType>
{
    public override void PostConfigure(EntityTypeBuilder<UnitType> builder)
    {
        builder.HasKey("Id");
    }
}