using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        builder.Property(a => a.EndTimeZone).HasMaxLength(32);
        builder.Property(a => a.Title).HasMaxLength(128);
        builder.Property(a => a.Description);
        builder.Property(a => a.Emoji).HasMaxLength(26);
        builder.Property(a => a.Color).HasMaxLength(40);
        builder
            .Property(a => a.StatusId)
            .HasConversion<EntityIdConverter<MilestoneStatusId, long>>()
            .ValueGeneratedNever();

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Project).WithMany(a => a.Milestones).HasForeignKey(a => a.ProjectId);
        builder
            .HasMany(a => a.Issues)
            .WithOne(a => a.Milestone)
            .HasForeignKey(a => a.MilestoneId)
            .OnDelete(DeleteBehavior.SetNull);
        builder
            .HasOne(a => a.Status)
            .WithMany(a => a.Milestones)
            .HasForeignKey(a => a.StatusId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(a => a.EndTime);
        builder.HasQueryFilter(a => a.Project.DeletedTime == null);
    }
}
