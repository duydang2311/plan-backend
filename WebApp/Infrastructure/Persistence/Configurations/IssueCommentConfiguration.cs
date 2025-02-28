using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueCommentConfiguration : IEntityTypeConfiguration<IssueComment>
{
    public void Configure(EntityTypeBuilder<IssueComment> builder)
    {
        builder.ToTable("issue_comments");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.Id).HasConversion<EntityGuidConverter<IssueCommentId>>().ValueGeneratedOnAdd();

        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Issue).WithMany(x => x.Comments).HasForeignKey(x => x.IssueId);
        builder.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorId);
        builder.HasIndex(x => x.IssueId);
        builder.HasQueryFilter(a => a.Issue.DeletedTime == null);
    }
}
