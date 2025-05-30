using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class MilestoneStatusConfiguration : IEntityTypeConfiguration<MilestoneStatus>
{
    public void Configure(EntityTypeBuilder<MilestoneStatus> builder)
    {
        builder.ToTable("milestone_statuses");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<MilestoneStatusId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Category).HasConversion<EnumToNumberConverter<MilestoneStatusCategory, byte>>();
        builder.Property(a => a.Rank).HasMaxLength(128).UseCollation("C");
        builder.Property(a => a.Value).HasMaxLength(64);
        builder.Property(a => a.Icon).HasMaxLength(64);
        builder.Property(a => a.Color).HasMaxLength(64);
        builder.Property(a => a.Description).HasMaxLength(256);
        builder.Property(a => a.ProjectId).HasConversion<EntityGuidConverter<ProjectId>>();
        builder.Property(a => a.IsDefault);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Rank);
        builder.HasOne(a => a.Project).WithMany(a => a.MilestoneStatuses).HasForeignKey(a => a.ProjectId);
    }
}
