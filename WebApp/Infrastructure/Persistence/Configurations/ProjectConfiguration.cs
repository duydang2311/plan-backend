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
        builder.Property(a => a.DeletedTime);
        builder.Property(a => a.Description);
        builder.HasGeneratedTsVectorColumn(
            a => a.SearchVector,
            "simple_unaccented",
            a => new
            {
                a.Name,
                a.Identifier,
                a.Description,
            }
        );

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.WorkspaceId, a.Identifier }).IsUnique();
        builder.HasIndex(a => a.DeletedTime);
        builder.HasIndex(a => a.SearchVector).HasMethod("gin");
        // builder
        //     .HasIndex(a => new { a.Identifier, a.Name })
        //     .HasMethod("gin")
        //     .HasOperators("gin_trgm_ops", "gin_trgm_ops");

        builder.HasOne(a => a.Workspace).WithMany(a => a.Projects).HasForeignKey(a => a.WorkspaceId);
        builder
            .HasMany(a => a.Teams)
            .WithMany(a => a.Projects)
            .UsingEntity<ProjectTeam>(
                r => r.HasOne(a => a.Team).WithMany().HasForeignKey(a => a.TeamId),
                l => l.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId)
            );
        builder.HasQueryFilter(a => a.DeletedTime == null);
    }
}
