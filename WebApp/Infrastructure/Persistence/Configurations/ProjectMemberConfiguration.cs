using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("project_members");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<ProjectMemberId, long>>().ValueGeneratedOnAdd();
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.ProjectId);
        builder.HasOne(a => a.User).WithMany(a => a.ProjectMembers).HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Role).WithMany().HasForeignKey(a => a.RoleId);
        builder.HasOne(a => a.Project).WithMany(a => a.Members).HasForeignKey(a => a.ProjectId);
    }
}
