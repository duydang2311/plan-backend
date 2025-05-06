using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceMemberConfiguration : IEntityTypeConfiguration<WorkspaceMember>
{
    public void Configure(EntityTypeBuilder<WorkspaceMember> builder)
    {
        builder.ToTable("workspace_members");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<WorkspaceMemberId, long>>().ValueGeneratedOnAdd();
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.CreatedTime);
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.WorkspaceId);
        builder.HasOne(a => a.Workspace).WithMany(a => a.Members).HasForeignKey(a => a.WorkspaceId);
        builder.HasOne(a => a.User).WithMany(a => a.WorkspaceMembers).HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Role).WithMany().HasForeignKey(a => a.RoleId);
    }
}
