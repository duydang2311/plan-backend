using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.Property(a => a.RoleId).HasConversion<EntityIdConverter<RoleId, int>>().ValueGeneratedNever();

        builder.HasKey(a => new { a.RoleId, a.Permission });
        builder.HasIndex(a => a.RoleId);

        builder.HasOne<Role>().WithMany(a => a.Permissions).HasForeignKey(a => a.RoleId);
    }
}
