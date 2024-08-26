using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("statuses");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<StatusId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Value).HasMaxLength(64);
        builder.Property(a => a.Color).HasMaxLength(16);
        builder.Property(a => a.Description).HasMaxLength(256);
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Workspace).WithMany(a => a.Statuses).HasForeignKey(a => a.WorkspaceId);
        builder.HasMany<Issue>().WithOne(a => a.Status).HasForeignKey(a => a.StatusId);
    }
}
