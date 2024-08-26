// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using WebApp.Domain.Entities;

// namespace WebApp.Infrastructure.Persistence.Configurations;

// public sealed class WorkspaceStatusConfiguration : IEntityTypeConfiguration<WorkspaceStatus>
// {
//     public void Configure(EntityTypeBuilder<WorkspaceStatus> builder)
//     {
//         builder.ToTable("workspace_statuses");
//         builder.Property(a => a.WorkspaceId).HasConversion<EntityGuidConverter<WorkspaceId>>();
//         builder.Property(a => a.StatusId).HasConversion<EntityIdConverter<StatusId, long>>();
//         builder.HasKey(a => new { a.WorkspaceId, a.StatusId });
//         // builder.HasOne(a => a.Workspace).WithMany(a => a.Statuses).HasForeignKey(a => a.WorkspaceId);
//         builder.HasOne(a => a.Status).WithOne().HasForeignKey<WorkspaceStatus>(a => a.StatusId);
//     }
// }
