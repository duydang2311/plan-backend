using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class ProjectResourceConfiguration : IEntityTypeConfiguration<ProjectResource>
{
    public void Configure(EntityTypeBuilder<ProjectResource> builder)
    {
        builder.ToTable("project_resources");

        builder.Property(a => a.ResourceId).HasConversion<EntityIdConverter<ResourceId, long>>().ValueGeneratedNever();
        builder.Property(a => a.ProjectId).HasConversion<EntityGuidConverter<ProjectId>>().ValueGeneratedNever();

        builder.HasKey(a => a.ResourceId);
        builder
            .HasOne(a => a.Resource)
            .WithOne(a => a.ProjectResource)
            .HasForeignKey<ProjectResource>(a => a.ResourceId);
        builder.HasQueryFilter(a => a.Project.DeletedTime == null);
    }
}
