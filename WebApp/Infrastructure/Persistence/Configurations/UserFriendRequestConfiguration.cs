using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserFriendRequestConfiguration : IEntityTypeConfiguration<UserFriendRequest>
{
    public void Configure(EntityTypeBuilder<UserFriendRequest> builder)
    {
        builder.ToTable(
            "user_friend_requests",
            a =>
            {
                a.HasCheckConstraint("CHK_user_friend_requests_sender_id_receiver_id", "sender_id < receiver_id");
            }
        );
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.SenderId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(x => x.ReceiverId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(x => new { x.SenderId, x.ReceiverId });
        builder.HasOne(a => a.Sender).WithMany().HasForeignKey(a => a.SenderId);
        builder.HasOne(a => a.Receiver).WithMany().HasForeignKey(a => a.ReceiverId);
    }
}
