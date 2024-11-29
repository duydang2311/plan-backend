using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Title).HasMaxLength(128);
        builder
            .Property(a => a.Priority)
            .HasConversion<EnumToNumberConverter<IssuePriority, byte>>()
            .HasDefaultValue(IssuePriority.None);

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Project).WithMany(a => a.Issues).HasForeignKey(a => a.ProjectId);
        builder.HasOne(a => a.Author).WithMany().HasForeignKey(a => a.AuthorId);
        builder.HasOne(a => a.Status).WithMany().HasForeignKey(a => a.StatusId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(a => new { a.ProjectId, a.OrderNumber }).IsUnique();
        builder.HasIndex(a => a.StatusId);
        builder.HasIndex(a => a.StatusRank);
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
