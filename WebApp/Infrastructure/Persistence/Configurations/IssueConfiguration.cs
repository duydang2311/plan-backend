using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.Id).HasConversion<EntityIdToGuidConverter<IssueId>>().ValueGeneratedOnAdd();
        builder.Property(x => x.Title).HasMaxLength(128);

        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Team).WithMany().HasForeignKey(x => x.TeamId);
        builder.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorId);
        builder.HasIndex(x => new { x.TeamId, x.OrderNumber }).IsUnique();
    }
}
