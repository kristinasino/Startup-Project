using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class RecycleInsertFormConfiguration : BaseEntityConfiguration<RecycleInsertForm>
{
    public override void PostConfigure(EntityTypeBuilder<RecycleInsertForm> builder)
    {
        builder.HasKey("Id");
    }
}