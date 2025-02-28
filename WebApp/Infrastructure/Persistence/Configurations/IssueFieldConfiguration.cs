using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class IssueFieldConfiguration : IEntityTypeConfiguration<IssueField>
{
    public void Configure(EntityTypeBuilder<IssueField> builder)
    {
        builder.ToTable("issue_fields");
        builder
            .UseTphMappingStrategy()
            .HasDiscriminator<FieldType>("discriminator")
            .HasValue<IssueFieldText>(FieldType.Text)
            .HasValue<IssueFieldNumber>(FieldType.Number)
            .HasValue<IssueFieldBoolean>(FieldType.Boolean);
        builder.Property(a => a.Name).HasMaxLength(32);
        builder.HasKey(a => new { a.IssueId, a.Name });
        builder.HasOne(a => a.Issue).WithMany(a => a.Fields).HasForeignKey(a => a.IssueId);
        builder.HasQueryFilter(a => a.Issue.DeletedTime == null);
    }
}
