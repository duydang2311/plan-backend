using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("user_sessions");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder
            .Property(a => a.SessionId)
            .HasConversion<EntityIdConverter<SessionId, string>>()
            .HasMaxLength(22)
            .ValueGeneratedNever();
        builder.Property(a => a.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(a => a.SessionId);
        builder.HasIndex(a => a.UserId);
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
    }
}
