using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueAuditConfiguration : IEntityTypeConfiguration<IssueAudit>
{
    public void Configure(EntityTypeBuilder<IssueAudit> builder)
    {
        builder.ToTable("issue_audits");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.IssueId).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedNever();
        builder.Property(a => a.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(a => a.Action);
        builder.Property(a => a.Data);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.IssueId);
        builder.HasOne(a => a.Issue).WithMany().HasForeignKey(a => a.IssueId);
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        builder.HasQueryFilter(a => !a.Issue.IsDeleted);
    }
}
