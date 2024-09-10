using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("statuses");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<StatusId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Value).HasMaxLength(64);
        builder.Property(a => a.Color).HasMaxLength(16);
        builder.Property(a => a.Description).HasMaxLength(256);

        builder.HasKey(a => a.Id);
        builder
            .HasMany<Workspace>()
            .WithMany(a => a.Statuses)
            .UsingEntity<WorkspaceStatus>(
                "workspace_statuses",
                l => l.HasOne(a => a.Workspace).WithMany().HasForeignKey(a => a.WorkspaceId),
                r => r.HasOne(a => a.Status).WithMany().HasForeignKey(a => a.StatusId),
                j => j.HasKey(a => a.StatusId)
            );
        builder
            .HasMany<Project>()
            .WithMany(a => a.Statuses)
            .UsingEntity<ProjectStatus>(
                "project_statuses",
                l => l.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId),
                r => r.HasOne(a => a.Status).WithMany().HasForeignKey(a => a.StatusId),
                j => j.HasKey(a => a.StatusId)
            );
    }
}
