using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.ToTable("user_notifications");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<UserNotificationId, long>>().ValueGeneratedOnAdd();
        builder
            .Property(a => a.NotificationId)
            .HasConversion<EntityIdConverter<NotificationId, long>>()
            .ValueGeneratedNever();
        builder.Property(a => a.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Ignore(a => a.IsRead);

        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Notification).WithMany().HasForeignKey(a => a.NotificationId);
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
    }
}
