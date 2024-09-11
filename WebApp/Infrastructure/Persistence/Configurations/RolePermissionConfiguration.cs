using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.Property(x => x.RoleId).HasConversion<EntityIdConverter<RoleId, int>>().ValueGeneratedNever();

        builder.HasKey(x => new { x.RoleId, x.Permission });
        builder.HasIndex(x => x.RoleId);

        builder.HasOne<Role>().WithMany(x => x.Permissions).HasForeignKey(x => x.RoleId);
    }
}
