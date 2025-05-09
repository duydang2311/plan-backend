using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueLinkConfiguration : IEntityTypeConfiguration<IssueLink>
{
    public void Configure(EntityTypeBuilder<IssueLink> builder)
    {
        builder.ToTable("issue_links");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<IssueLinkId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.SubIssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();

        builder.HasKey(a => a.IssueId);
        builder.HasOne(a => a.Issue).WithMany(a => a.SubIssues).HasForeignKey(a => a.IssueId);
        builder.HasOne(a => a.SubIssue).WithMany(a => a.ParentIssues).HasForeignKey(a => a.SubIssueId);
        builder.HasQueryFilter(a => a.Issue.DeletedTime == null);
    }
}
