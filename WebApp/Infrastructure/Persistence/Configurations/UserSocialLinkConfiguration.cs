using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserSocialLinkConfiguration : IEntityTypeConfiguration<UserSocialLink>
{
    public void Configure(EntityTypeBuilder<UserSocialLink> builder)
    {
        builder.ToTable("user_social_links");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(a => a.Url).HasMaxLength(256);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.UserId);
        builder.HasOne(a => a.Profile).WithMany(a => a.SocialLinks).HasForeignKey(a => a.UserId);
    }
}
