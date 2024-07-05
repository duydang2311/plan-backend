using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");
        builder.Property(x => x.CreatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedTime).HasDefaultValueSql("now()");
        builder.Property(x => x.Id).HasConversion<TeamIdToGuidConverter>().ValueGeneratedOnAdd();

        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Workspace).WithMany().HasForeignKey(x => x.WorkspaceId);
        builder.HasMany(x => x.Members).WithMany(x => x.Teams).UsingEntity("team_members");
    }
}
