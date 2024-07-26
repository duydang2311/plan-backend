using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("workspaces");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.Id).HasConversion<EntityGuidConverter<WorkspaceId>>().ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(64);
        builder.Property(x => x.Path).HasMaxLength(64).UseCollation("case_insensitive");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Path).IsUnique();
    }
}
