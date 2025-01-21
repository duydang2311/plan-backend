using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("team_members");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.TeamId).HasConversion<EntityGuidConverter<TeamId>>();
        builder.Property(x => x.MemberId).HasConversion<EntityGuidConverter<UserId>>();
        builder.Property(x => x.RoleId).HasConversion<EntityIdConverter<TeamRoleId, int>>();

        builder.HasKey(x => new { x.TeamId, x.MemberId });
        builder.HasOne(x => x.Team).WithMany(a => a.TeamMembers).HasForeignKey(x => x.TeamId);
        builder.HasOne(x => x.Member).WithMany().HasForeignKey(x => x.MemberId);
        builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
    }
}
