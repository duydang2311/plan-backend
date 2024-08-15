using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class TeamInvitationConfiguration : IEntityTypeConfiguration<TeamInvitation>
{
    public void Configure(EntityTypeBuilder<TeamInvitation> builder)
    {
        builder.ToTable("team_invitations");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.TeamId).HasConversion<EntityGuidConverter<TeamId>>().ValueGeneratedNever();
        builder.Property(x => x.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(a => new { a.TeamId, a.UserId });
        builder.HasOne(a => a.Team).WithMany().HasForeignKey(a => a.TeamId);
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        builder
            .HasOne<TeamMember>()
            .WithOne(a => a.PendingInvitation)
            .HasForeignKey<TeamInvitation>(a => new { a.TeamId, a.UserId });
    }
}
