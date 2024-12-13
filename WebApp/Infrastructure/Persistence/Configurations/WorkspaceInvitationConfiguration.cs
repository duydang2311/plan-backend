using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceInvitationConfiguration : IEntityTypeConfiguration<WorkspaceInvitation>
{
    public void Configure(EntityTypeBuilder<WorkspaceInvitation> builder)
    {
        builder.ToTable("workspace_invitations");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder
            .Property(x => x.Id)
            .HasConversion<EntityIdConverter<WorkspaceInvitationId, long>>()
            .ValueGeneratedOnAdd();
        builder.Property(x => x.WorkspaceId).HasConversion<EntityGuidConverter<WorkspaceId>>().ValueGeneratedNever();
        builder.Property(x => x.UserId).HasConversion<EntityGuidConverter<UserId>>().ValueGeneratedNever();

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.WorkspaceId, a.UserId }).IsUnique();
        builder.HasOne(a => a.Workspace).WithMany().HasForeignKey(a => a.WorkspaceId);
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
    }
}
