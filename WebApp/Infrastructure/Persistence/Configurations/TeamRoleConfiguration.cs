using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class TeamRoleConfiguration : IEntityTypeConfiguration<TeamRole>
{
    public void Configure(EntityTypeBuilder<TeamRole> builder)
    {
        builder.ToTable("team_roles");
        builder.Property(x => x.Id).HasConversion<EntityIdConverter<TeamRoleId, int>>();
        builder.Property(x => x.Name).HasMaxLength(32);

        builder.HasKey(x => x.Id);
    }
}
