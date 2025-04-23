using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("resources");
        builder.UseTphMappingStrategy();
        builder
            .HasDiscriminator(a => a.Type)
            .HasValue<DocumentResource>(ResourceType.Document)
            .HasValue<FileResource>(ResourceType.File);

        builder.Property(a => a.Id).HasConversion<EntityIdConverter<ResourceId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Type).HasConversion<EnumToNumberConverter<ResourceType, byte>>();
        builder.Property(a => a.CreatorId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(a => a.Name).HasMaxLength(256);
        builder.Property(a => a.Rank).UseCollation("C");

        builder.HasOne(a => a.Creator).WithMany().HasForeignKey(a => a.CreatorId);
    }
}
