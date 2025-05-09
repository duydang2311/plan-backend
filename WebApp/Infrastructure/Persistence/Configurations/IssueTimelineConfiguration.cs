using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueTimelineConfiguration : IEntityTypeConfiguration<IssueTimeline>
{
    public void Configure(EntityTypeBuilder<IssueTimeline> builder)
    {
        builder.ToTable("issue_timelines");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.StartTime);
        builder.Property(a => a.EndTime);

        builder.HasKey(a => a.IssueId);
        builder.HasOne(a => a.Issue).WithOne(a => a.Timeline).HasForeignKey<IssueTimeline>(a => a.IssueId);
        builder.HasQueryFilter(a => a.Issue.DeletedTime == null);
    }
}
