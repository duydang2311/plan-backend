using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ResourceDocumentConfiguration : IEntityTypeConfiguration<ResourceDocument>
{
    public void Configure(EntityTypeBuilder<ResourceDocument> builder)
    {
        builder.ToTable("resource_documents");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ResourceId).HasConversion<EntityIdConverter<ResourceId, long>>().ValueGeneratedNever();
        builder.Property(a => a.Content);

        builder.HasKey(a => a.ResourceId);
        builder.HasOne(a => a.Resource).WithOne(a => a.Document).HasForeignKey<ResourceDocument>(a => a.ResourceId);
    }
}
