using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<NotificationId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Type).HasConversion<EnumToNumberConverter<NotificationType, byte>>();
        builder.Property(a => a.Data);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Type);
    }
}
