using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.Property(a => a.UserRoleId).HasConversion<EntityIdConverter<UserRoleId, long>>().ValueGeneratedOnAdd();
        builder.UseTphMappingStrategy().HasDiscriminator<string>("role_type").HasValue<ProjectMember>("project");
        builder.HasOne(a => a.User).WithMany(a => a.Roles).HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Role).WithMany().HasForeignKey(a => a.RoleId);
        builder.HasKey(a => a.UserRoleId);
        builder.HasIndex(a => a.UserId);
    }
}
