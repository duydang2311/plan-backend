using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class MemberRolePermissionConfiguration : IEntityTypeConfiguration<TeamRolePermission>
{
    public void Configure(EntityTypeBuilder<TeamRolePermission> builder)
    {
        builder.ToTable("team_role_permissions");
        builder.Property(x => x.RoleId).HasConversion<EntityIdConverter<TeamRoleId, int>>().ValueGeneratedOnAdd();

        builder.HasKey(x => new { x.RoleId, x.Permission });
        builder.HasIndex(x => x.RoleId);

        builder.HasOne<TeamRole>().WithMany(x => x.Permissions).HasForeignKey(x => x.RoleId);
    }
}
