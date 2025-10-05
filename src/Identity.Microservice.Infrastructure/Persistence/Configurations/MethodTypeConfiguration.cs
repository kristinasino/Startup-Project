using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Domain.Entities;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class MethodTypeConfiguration : BaseEntityConfiguration<MethodType>
{
    public override void PostConfigure(EntityTypeBuilder<MethodType> builder)
    {
        builder.HasKey("Id");
    }
}