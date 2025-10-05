using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class TonageInsertFormConfiguration : BaseEntityConfiguration<TonageInsertForm>
{
    public override void PostConfigure(EntityTypeBuilder<TonageInsertForm> builder)
    {
        builder.HasKey("Id");
    }
}