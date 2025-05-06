using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.Property(a => a.Id).HasConversion<EntityIdConverter<RoleId, int>>();
        builder.Property(a => a.Name).HasMaxLength(32);
        builder.Property(a => a.Rank);

        builder.HasKey(x => x.Id);
    }
}
