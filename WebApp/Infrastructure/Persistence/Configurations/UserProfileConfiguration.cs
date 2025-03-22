using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(a => a.Name).HasMaxLength(64);
        builder.ComplexProperty(
            a => a.Image,
            b =>
            {
                b.Property(a => a.ResourceType).HasMaxLength(16);
                b.Property(a => a.PublicId).HasMaxLength(256);
                b.Property(a => a.Format).HasMaxLength(16);
                b.Property(a => a.Version);
            }
        );
        builder.Property(a => a.DisplayName).HasMaxLength(64);
        builder.Property(a => a.Bio).HasMaxLength(256);
        builder.Property(a => a.Trigrams).HasComputedColumnSql("\"name\" || ' ' || \"display_name\"", stored: true);

        builder.HasKey(a => a.UserId);
        builder.HasIndex(a => a.Name).IsUnique();
        builder.HasIndex(a => a.Trigrams).HasMethod("gin").HasOperators("gin_trgm_ops");
        builder.HasOne(a => a.User).WithOne(a => a.Profile).HasForeignKey<UserProfile>(a => a.UserId);
    }
}
