using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
{
    public void Configure(EntityTypeBuilder<Milestone> builder)
    {
        builder.ToTable("milestones");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<MilestoneId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.EndTime);
        builder.Property(a => a.Title).HasMaxLength(128);
        builder.Property(a => a.Description);
        builder.Property(a => a.PreviewDescription).HasMaxLength(256);
        builder.Property(a => a.Emoji).HasMaxLength(26);
        builder.Property(a => a.Color).HasMaxLength(40);

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Project).WithMany(a => a.Milestones).HasForeignKey(a => a.ProjectId);
        builder.HasMany(a => a.Issues).WithOne(a => a.Milestone).HasForeignKey(a => a.MilestoneId);
        builder.HasQueryFilter(a => a.Project.DeletedTime == null);
    }
}
