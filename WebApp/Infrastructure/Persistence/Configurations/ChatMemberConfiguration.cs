using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
{
    public void Configure(EntityTypeBuilder<ChatMember> builder)
    {
        builder.ToTable("chat_members");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.ChatId).HasConversion<EntityGuidConverter<ChatId>>().ValueGeneratedNever();
        builder.Property(a => a.MemberId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(a => a.LastReadMessageId).HasConversion<EntityIdConverter<ChatMessageId, long>>();

        builder.HasKey(a => new { a.ChatId, a.MemberId });
        builder.HasOne(a => a.Chat).WithMany(a => a.ChatMembers).HasForeignKey(a => a.ChatId);
        builder.HasOne(a => a.Member).WithMany(a => a.ChatMembers).HasForeignKey(a => a.MemberId);
        builder.HasQueryFilter(a => a.Chat.DeletedTime == null);
    }
}
