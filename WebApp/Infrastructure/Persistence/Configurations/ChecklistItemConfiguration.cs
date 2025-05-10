using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> builder)
    {
        builder.ToTable("checklist_items");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<ChecklistItemId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ParentIssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.SubIssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.Content).HasMaxLength(256);

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.ParentIssue).WithMany(a => a.SubChecklistItems).HasForeignKey(a => a.ParentIssueId);
        builder.HasOne(a => a.SubIssue).WithMany(a => a.ParentChecklistItems).HasForeignKey(a => a.SubIssueId);
        builder.HasIndex(a => a.SubIssueId);
        builder.HasQueryFilter(a =>
            a.ParentIssue.DeletedTime == null && (a.SubIssue == null || a.SubIssue.DeletedTime == null)
        );
    }
}
