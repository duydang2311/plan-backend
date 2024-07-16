using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class UserVerificationTokenConfiguration : IEntityTypeConfiguration<UserVerificationToken>
{
    public void Configure(EntityTypeBuilder<UserVerificationToken> builder)
    {
        builder.ToTable("user_verification_tokens");
        builder.Property(x => x.UserId).HasConversion<UserIdToGuidConverter>();
        builder.Property(x => x.Token).ValueGeneratedOnAdd();
        builder
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<UserVerificationToken>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasKey(x => new { x.UserId, x.Token });
    }
}
