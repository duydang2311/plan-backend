using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectTeamConfiguration : IEntityTypeConfiguration<ProjectTeam>
{
    public void Configure(EntityTypeBuilder<ProjectTeam> builder)
    {
        builder.ToTable("project_teams");
        builder.Property(a => a.ProjectId).HasConversion<EntityGuidConverter<ProjectId>>();
        builder.Property(a => a.TeamId).HasConversion<EntityGuidConverter<TeamId>>();

        builder.HasKey(a => new { a.ProjectId, a.TeamId });
        builder.HasOne(a => a.Project).WithMany().HasForeignKey(a => a.ProjectId);
        builder.HasOne(a => a.Team).WithMany().HasForeignKey(a => a.TeamId);
    }
}
