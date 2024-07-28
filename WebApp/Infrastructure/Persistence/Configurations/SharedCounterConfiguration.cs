using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class SharedCounterConfiguration : IEntityTypeConfiguration<SharedCounter>
{
    public void Configure(EntityTypeBuilder<SharedCounter> builder)
    {
        builder.ToTable("shared_counters");
        builder.HasKey(x => x.Id);
    }
}
