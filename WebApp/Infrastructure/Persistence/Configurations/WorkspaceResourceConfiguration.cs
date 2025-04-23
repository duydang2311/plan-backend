using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceResourceConfiguration : IEntityTypeConfiguration<WorkspaceResource>
{
    public void Configure(EntityTypeBuilder<WorkspaceResource> builder)
    {
        builder.ToTable("workspace_resources");

        builder.Property(a => a.ResourceId).HasConversion<EntityIdConverter<ResourceId, long>>().ValueGeneratedNever();
        builder.Property(a => a.WorkspaceId).HasConversion<EntityGuidConverter<WorkspaceId>>().ValueGeneratedNever();

        builder.HasKey(a => a.ResourceId);
        builder.HasOne(a => a.Resource).WithOne().HasForeignKey<WorkspaceResource>(a => a.ResourceId);
        builder.HasOne(a => a.Workspace).WithMany().HasForeignKey(a => a.WorkspaceId);
        builder.HasQueryFilter(a => a.Workspace.DeletedTime == null);
    }
}
