using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class DocumentResourceConfiguration : IEntityTypeConfiguration<DocumentResource>
{
    public void Configure(EntityTypeBuilder<DocumentResource> builder)
    {
        builder.Property(a => a.Content);
    }
}
