using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities.Models.Audits;

namespace UserModule.Infrastructure.Persistence.Configurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
