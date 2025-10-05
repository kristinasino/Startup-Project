using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class ContractTypeConfiguration : BaseEntityConfiguration<ContractType>
{
    public override void PostConfigure(EntityTypeBuilder<ContractType> builder)
    {
        builder.HasKey("Id");
    }
}