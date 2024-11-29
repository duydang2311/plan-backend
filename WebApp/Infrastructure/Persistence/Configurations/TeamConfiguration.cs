using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.Id).HasConversion<EntityGuidConverter<TeamId>>().ValueGeneratedOnAdd();
        builder.Property(x => x.Identifier).HasMaxLength(5).UseCollation("case_insensitive");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.WorkspaceId, x.Identifier }).IsUnique();
        builder.HasOne(x => x.Workspace).WithMany().HasForeignKey(x => x.WorkspaceId);
        builder
            .HasMany(x => x.Members)
            .WithMany(x => x.Teams)
            .UsingEntity<TeamMember>(
                l => l.HasOne<User>().WithMany().HasForeignKey(x => x.MemberId),
                r => r.HasOne<Team>().WithMany().HasForeignKey(x => x.TeamId)
            );
        builder
            .HasMany(x => x.Issues)
            .WithMany(x => x.Teams)
            .UsingEntity<TeamIssue>(
                l => l.HasOne<Issue>().WithMany(a => a.TeamIssues).HasForeignKey(a => a.IssueId),
                r => r.HasOne<Team>().WithMany(a => a.TeamIssues).HasForeignKey(a => a.TeamId)
            );
    }
}
