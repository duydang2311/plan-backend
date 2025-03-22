using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Email).HasMaxLength(254);
        builder.Property(a => a.Salt);
        builder.Property(a => a.PasswordHash);
        builder.Property(a => a.IsVerified).HasDefaultValue(false);
        builder.Property(a => a.Trigrams).HasComputedColumnSql("\"email\"", stored: true);

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Email);
        builder.HasIndex(a => a.Trigrams).HasMethod("gin").HasOperators("gin_trgm_ops");
        builder.HasMany(a => a.Issues).WithMany(a => a.Assignees).UsingEntity<IssueAssignee>();
        builder.HasMany(a => a.Workspaces).WithMany(a => a.Users).UsingEntity<WorkspaceMember>();
    }
}
