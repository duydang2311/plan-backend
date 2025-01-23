using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectMemberInvitationConfiguration : IEntityTypeConfiguration<ProjectMemberInvitation>
{
    public void Configure(EntityTypeBuilder<ProjectMemberInvitation> builder)
    {
        builder.ToTable("project_member_invitations");
        builder.HasKey(a => new { a.UserId, a.ProjectId });
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.ProjectId);
        builder.HasOne(a => a.Role).WithMany().HasForeignKey(a => a.RoleId);
        builder.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId);
    }
}
