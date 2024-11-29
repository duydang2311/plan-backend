using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class TeamIssueConfiguration : IEntityTypeConfiguration<TeamIssue>
{
    public void Configure(EntityTypeBuilder<TeamIssue> builder)
    {
        builder.ToTable("team_issues");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.TeamId).HasConversion<EntityGuidConverter<TeamId>>();
        builder.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>();
        builder.Property(a => a.Rank).UseCollation("C");

        builder.HasKey(a => new { a.TeamId, a.IssueId });
        builder.HasOne(a => a.Team).WithMany(a => a.TeamIssues).HasForeignKey(a => a.TeamId);
        builder.HasOne(a => a.Issue).WithMany(a => a.TeamIssues).HasForeignKey(a => a.IssueId);
        builder.HasQueryFilter(a => !a.Issue.IsDeleted);
    }
}
