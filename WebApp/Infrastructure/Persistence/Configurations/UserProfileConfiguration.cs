using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(64);
        builder.Property(x => x.ImageUrl).HasMaxLength(2000);

        builder.HasKey(a => a.UserId);
        builder.HasIndex(a => a.Name).IsUnique();
        builder.HasOne(a => a.User).WithOne(a => a.Profile).HasForeignKey<UserProfile>(a => a.UserId);
    }
}
