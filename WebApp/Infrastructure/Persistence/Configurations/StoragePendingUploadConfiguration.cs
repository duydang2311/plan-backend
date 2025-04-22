using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class StoragePendingUploadConfiguration : IEntityTypeConfiguration<StoragePendingUpload>
{
    public void Configure(EntityTypeBuilder<StoragePendingUpload> builder)
    {
        builder.ToTable("storage_pending_uploads");
        builder
            .Property(a => a.Id)
            .HasConversion<EntityIdConverter<StoragePendingUploadId, long>>()
            .ValueGeneratedOnAdd();
        builder.Property(a => a.Key).HasMaxLength(1024);
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ExpiryTime);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.ExpiryTime);
    }
}
