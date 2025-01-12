using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.Id).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedOnAdd();
        builder.Property(x => x.Email).HasMaxLength(254);
        builder.Property(x => x.Salt);
        builder.Property(x => x.PasswordHash);
        builder.Property(x => x.IsVerified).HasDefaultValue(false);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email).HasMethod("gin").HasOperators("gin_trgm_ops");
        builder.HasMany(a => a.Issues).WithMany(a => a.Assignees).UsingEntity<IssueAssignee>();
        builder.HasMany(a => a.Workspaces).WithMany(a => a.Users).UsingEntity<WorkspaceMember>();
    }
}
