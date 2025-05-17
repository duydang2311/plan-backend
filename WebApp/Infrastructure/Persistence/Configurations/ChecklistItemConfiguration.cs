using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> builder)
    {
        builder.ToTable(
            "checklist_items",
            a =>
            {
                a.HasCheckConstraint(
                    "CHK_valid_todo",
                    $"(\"kind\" = {(byte)ChecklistItemKind.Todo} AND \"content\" IS NOT NULL AND \"completed\" IS NOT NULL AND \"sub_issue_id\" IS NULL) OR (\"kind\" != {(byte)ChecklistItemKind.Todo} AND \"content\" IS NULL AND \"completed\" IS NULL)"
                );
                a.HasCheckConstraint(
                    "CHK_valid_sub_issue",
                    $"(\"kind\" = {(byte)ChecklistItemKind.SubIssue} AND \"sub_issue_id\" IS NOT NULL) OR (\"kind\" != {(byte)ChecklistItemKind.SubIssue} AND \"sub_issue_id\" IS NULL)"
                );
            }
        );
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<ChecklistItemId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ParentIssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.Kind).HasConversion<EnumToNumberConverter<ChecklistItemKind, byte>>();
        builder.Property(a => a.SubIssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.Content).HasMaxLength(256);
        builder.Property(a => a.Completed);

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.ParentIssue).WithMany(a => a.SubChecklistItems).HasForeignKey(a => a.ParentIssueId);
        builder.HasOne(a => a.SubIssue).WithMany(a => a.ParentChecklistItems).HasForeignKey(a => a.SubIssueId);
        builder.HasIndex(a => a.SubIssueId);
        builder.HasIndex(a => new { a.ParentIssueId, a.SubIssueId }).IsUnique();
        builder.HasQueryFilter(a =>
            a.ParentIssue.DeletedTime == null && (a.SubIssue == null || a.SubIssue.DeletedTime == null)
        );
    }
}
