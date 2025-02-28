using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueAssigneeConfiguration : IEntityTypeConfiguration<IssueAssignee>
{
    public void Configure(EntityTypeBuilder<IssueAssignee> builder)
    {
        builder.ToTable("issue_assignees");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>();
        builder.Property(a => a.UserId).HasConversion<EntityGuidConverter<UserId>>();

        builder.HasKey(a => new { a.IssueId, a.UserId });
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Issue).WithMany(a => a.IssueAssignees).HasForeignKey(a => a.IssueId);
        builder.HasQueryFilter(a => a.Issue.DeletedTime == null);
    }
}
