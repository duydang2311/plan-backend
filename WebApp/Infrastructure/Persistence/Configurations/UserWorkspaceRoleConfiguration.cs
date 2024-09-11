using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserWorkspaceRoleConfiguration : IEntityTypeConfiguration<UserWorkspaceRole>
{
    public void Configure(EntityTypeBuilder<UserWorkspaceRole> builder)
    {
        builder.HasOne(a => a.Workspace).WithMany().HasForeignKey(a => a.WorkspaceId);
        builder.HasIndex(a => a.WorkspaceId);
    }
}
