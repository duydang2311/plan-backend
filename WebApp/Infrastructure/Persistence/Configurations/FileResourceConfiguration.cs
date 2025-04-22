using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class FileResourceConfiguration : IEntityTypeConfiguration<FileResource>
{
    public void Configure(EntityTypeBuilder<FileResource> builder)
    {
        builder.Property(a => a.Key).HasMaxLength(1024);
    }
}
