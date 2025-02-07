using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectMemberInvitationConfiguration : IEntityTypeConfiguration<ProjectMemberInvitation>
{
    public void Configure(EntityTypeBuilder<ProjectMemberInvitation> builder)
    {
        builder.ToTable("project_member_invitations");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder
            .Property(a => a.Id)
            .HasConversion<EntityIdConverter<ProjectMemberInvitationId, long>>()
            .ValueGeneratedOnAdd();
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.UserId, a.ProjectId }).IsUnique();
        builder.HasOne(a => a.Role).WithMany().HasForeignKey(a => a.RoleId);
        builder.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId);
    }
}
