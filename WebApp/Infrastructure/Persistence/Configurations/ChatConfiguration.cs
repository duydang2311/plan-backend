using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable(
            "chats",
            a =>
            {
                a.HasCheckConstraint(
                    "CHK_valid_title",
                    "(\"type\" = 1 AND \"title\" IS NULL) OR (\"type\" != 1 AND \"title\" IS NOT NULL)"
                );
            }
        );
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<ChatId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Type).HasConversion<EnumToNumberConverter<ChatType, byte>>();
        builder.Property(a => a.Title).HasMaxLength(128);
        builder.Property(a => a.DeletedTime);
        builder.Property(a => a.OwnerId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();
        builder.Ignore(a => a.LastChatMessage);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.DeletedTime);
        builder.HasMany(a => a.Members).WithMany(a => a.Chats).UsingEntity<ChatMember>();
        builder.HasOne(a => a.Owner).WithMany(a => a.OwnedChats).HasForeignKey(a => a.OwnerId);
        builder.HasQueryFilter(a => a.DeletedTime == null);
    }
}
