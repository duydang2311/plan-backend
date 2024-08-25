using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");
        builder.Property(a => a.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<IssueId>>().ValueGeneratedOnAdd();
        builder.Property(a => a.Title).HasMaxLength(128);

        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Team).WithMany(x => x.Issues).HasForeignKey(x => x.TeamId);
        builder.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorId);
        builder.HasIndex(x => new { x.TeamId, x.OrderNumber }).IsUnique();
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
