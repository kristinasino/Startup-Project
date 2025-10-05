using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class CustomerLocationEquipmentConfiguration : BaseEntityConfiguration<CustomerLocationEquipment>
{
    public override void PostConfigure(EntityTypeBuilder<CustomerLocationEquipment> builder)
    {
        builder.HasKey("Id");
    }
}