using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserFriendConfiguration : IEntityTypeConfiguration<UserFriend>
{
    public void Configure(EntityTypeBuilder<UserFriend> builder)
    {
        builder.ToTable(
            "user_friends",
            a =>
            {
                a.HasCheckConstraint("CHK_user_friends_user_id_friend_id", "user_id < friend_id");
            }
        );
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(x => x.FriendId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(x => new { x.UserId, x.FriendId });
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Friend).WithMany().HasForeignKey(a => a.FriendId);
    }
}
