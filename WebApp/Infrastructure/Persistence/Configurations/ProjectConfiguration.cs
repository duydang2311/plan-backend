using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<ProjectId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Name).HasMaxLength(64);
        builder.Property(a => a.Identifier).HasMaxLength(64).UseCollation("case_insensitive");

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.WorkspaceId, a.Identifier }).IsUnique();

        builder.HasOne(a => a.Workspace).WithMany(a => a.Projects).HasForeignKey(a => a.WorkspaceId);
        builder
            .HasMany(a => a.Issues)
            .WithMany(a => a.Projects)
            .UsingEntity<ProjectIssue>(
                "project_issues",
                r => r.HasOne(a => a.Issue).WithMany().HasForeignKey(a => a.IssueId),
                l => l.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId),
                b =>
                {
                    b.Property(a => a.ProjectId).HasConversion<EntityGuidConverter<ProjectId>>();
                    b.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>();
                    b.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
                    b.HasQueryFilter(a => !a.Issue.IsDeleted);
                }
            );
        builder
            .HasMany(a => a.Teams)
            .WithMany(a => a.Projects)
            .UsingEntity<ProjectTeam>(
                "project_teams",
                r => r.HasOne(a => a.Team).WithMany().HasForeignKey(a => a.TeamId),
                l => l.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId),
                b =>
                {
                    b.Property(a => a.ProjectId).HasConversion<EntityGuidConverter<ProjectId>>();
                    b.Property(a => a.TeamId).HasConversion<EntityGuidConverter<TeamId>>();
                }
            );
    }
}
