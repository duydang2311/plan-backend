using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserGoogleAuthConfiguration : IEntityTypeConfiguration<UserGoogleAuth>
{
    public void Configure(EntityTypeBuilder<UserGoogleAuth> builder)
    {
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(x => x.GoogleId).HasMaxLength(64);
        builder.Property(x => x.Email).HasMaxLength(254);

        builder.HasKey(x => x.UserId);
        builder.HasOne(a => a.User).WithOne(a => a.GoogleAuth).HasForeignKey<UserGoogleAuth>(a => a.UserId);
    }
}
