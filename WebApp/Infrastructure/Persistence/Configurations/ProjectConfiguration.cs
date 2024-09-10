using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<ProjectId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Name).HasMaxLength(64);
        builder.Property(a => a.Identifier).HasMaxLength(64).UseCollation("case_insensitive");

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.WorkspaceId, a.Identifier }).IsUnique();

        builder.HasOne(a => a.Workspace).WithMany().HasForeignKey(a => a.WorkspaceId);
    }
}