using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("workspaces");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<WorkspaceId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Name).HasMaxLength(64);
        builder.Property(a => a.Path).HasMaxLength(64).UseCollation("case_insensitive");
        builder.Property(a => a.DeletedTime);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Path).IsUnique();
        builder.HasIndex(a => a.DeletedTime);

        builder.HasQueryFilter(a => a.DeletedTime == null);
    }
}
