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
        builder.Property(x => x.Id).HasConversion<EntityGuidConverter<TeamInvitationId>>().ValueGeneratedOnAdd();
        builder.Property(x => x.TeamId).HasConversion<EntityGuidConverter<TeamId>>().ValueGeneratedNever();
        builder.Property(x => x.MemberId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.TeamId, a.MemberId }).IsUnique();
        builder.HasOne(a => a.Team).WithMany().HasForeignKey(a => a.TeamId);
        builder.HasOne(a => a.Member).WithMany().HasForeignKey(a => a.MemberId);
        builder
            .HasOne<TeamMember>()
            .WithOne(a => a.PendingInvitation)
            .HasForeignKey<TeamInvitation>(a => new { a.TeamId, a.MemberId });
    }
}
