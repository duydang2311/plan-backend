using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceFieldDefinitionConfiguration : IEntityTypeConfiguration<WorkspaceFieldDefinition>
{
    public void Configure(EntityTypeBuilder<WorkspaceFieldDefinition> builder)
    {
        builder.ToTable("workspace_field_definitions");
        builder.Property(a => a.Name).HasMaxLength(64);
        builder.Property(a => a.Type).HasConversion<EnumToStringConverter<FieldType>>().HasMaxLength(16);
        builder.Property(a => a.Description).HasMaxLength(256);
        builder.HasKey(a => new { a.WorkspaceId, a.Name });
        builder.HasOne(a => a.Workspace).WithMany(a => a.FieldDefinitions).HasForeignKey(a => a.WorkspaceId);
    }
}
