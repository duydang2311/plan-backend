using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserNotificationReadFlagConfiguration : IEntityTypeConfiguration<UserNotificationReadFlag>
{
    public void Configure(EntityTypeBuilder<UserNotificationReadFlag> builder)
    {
        builder.ToTable("user_notification_read_flags");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder
            .Property(a => a.UserNotificationId)
            .HasConversion<EntityIdConverter<UserNotificationId, long>>()
            .ValueGeneratedNever();

        builder.HasKey(a => a.UserNotificationId);
        builder
            .HasOne(a => a.UserNotification)
            .WithOne(a => a.ReadFlag)
            .HasForeignKey<UserNotificationReadFlag>(a => a.UserNotificationId);
    }
}
