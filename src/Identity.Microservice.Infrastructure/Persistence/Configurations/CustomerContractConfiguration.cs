using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class CustomerContractConfiguration : BaseEntityConfiguration<CustomerContract>
{
    public override void PostConfigure(EntityTypeBuilder<CustomerContract> builder)
    {
        builder.HasKey("Id");
    }
}