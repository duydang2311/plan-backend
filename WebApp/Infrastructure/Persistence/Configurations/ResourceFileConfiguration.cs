using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ResourceFileConfiguration : IEntityTypeConfiguration<ResourceFile>
{
    public void Configure(EntityTypeBuilder<ResourceFile> builder)
    {
        builder.ToTable("resource_files");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<ResourceFileId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ResourceId).HasConversion<EntityIdConverter<ResourceId, long>>().ValueGeneratedNever();
        builder.Property(a => a.Key).HasMaxLength(1024);
        builder.Property(a => a.OriginalName).HasMaxLength(255);

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Resource).WithMany(a => a.Files).HasForeignKey(a => a.ResourceId);
    }
}
