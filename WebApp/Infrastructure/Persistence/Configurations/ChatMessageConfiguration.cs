using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("chat_messages");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<ChatMessageId, long>>().ValueGeneratedOnAdd();
        builder.Property(a => a.ChatId).HasConversion<EntityGuidConverter<ChatId>>().ValueGeneratedNever();
        builder.Property(a => a.SenderId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Property(a => a.Content);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.CreatedTime);
        builder.HasOne(a => a.Chat).WithMany(a => a.ChatMessages).HasForeignKey(a => a.ChatId);
        builder.HasOne(a => a.Sender).WithMany().HasForeignKey(a => a.SenderId);
        builder.HasQueryFilter(a => a.Chat.DeletedTime == null);
    }
}
