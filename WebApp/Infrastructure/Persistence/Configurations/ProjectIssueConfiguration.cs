using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectIssueConfiguration : IEntityTypeConfiguration<ProjectIssue>
{
    public void Configure(EntityTypeBuilder<ProjectIssue> builder)
    {
        builder.ToTable("project_issues");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ProjectId).HasConversion<EntityGuidConverter<ProjectId>>();
        builder.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>();
        builder.Property(a => a.Rank).UseCollation("C");

        builder.HasKey(a => new { a.ProjectId, a.IssueId });
        builder.HasOne(a => a.Project).WithMany(a => a.ProjectIssues).HasForeignKey(a => a.ProjectId);
        builder.HasOne(a => a.Issue).WithMany(a => a.ProjectIssues).HasForeignKey(a => a.IssueId);
        builder.HasQueryFilter(a => !a.Issue.IsDeleted);
    }
}
