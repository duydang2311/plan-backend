using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable("policies");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Subject).HasMaxLength(64);
        builder.Property(x => x.Object).HasMaxLength(64);
        builder.Property(x => x.Action).HasMaxLength(64);
        builder.Property(x => x.Domain).HasMaxLength(64);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Subject);
        builder.HasIndex(x => x.Object);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.Domain);
    }
}
