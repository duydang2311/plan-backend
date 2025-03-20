using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserFriendRequestConfiguration : IEntityTypeConfiguration<UserFriendRequest>
{
    public void Configure(EntityTypeBuilder<UserFriendRequest> builder)
    {
        builder.ToTable("user_friend_requests");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.SenderId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(x => x.ReceiverId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(x => new { x.SenderId, x.ReceiverId });
        builder.HasOne(a => a.Sender).WithMany(a => a.UserSentFriendRequests).HasForeignKey(a => a.SenderId);
        builder.HasOne(a => a.Receiver).WithMany(a => a.UserReceivedFriendRequests).HasForeignKey(a => a.ReceiverId);
    }
}
