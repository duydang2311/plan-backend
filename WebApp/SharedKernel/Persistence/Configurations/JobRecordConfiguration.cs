using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Entities;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class JobRecordConfiguration : IEntityTypeConfiguration<JobRecord>
{
    public void Configure(EntityTypeBuilder<JobRecord> builder)
    {
        builder.ToTable("job_records");
        builder.Property(x => x.TrackingID).ValueGeneratedOnAdd();
        builder.Property(x => x.QueueID).HasMaxLength(32);
        builder.Property(x => x.CommandJson).HasColumnType("jsonb");
        builder.Property(x => x.ExecuteAfter);
        builder.Property(x => x.ExpireOn);
        builder.Property(x => x.IsComplete);

        builder.HasKey(x => x.TrackingID);
        builder.Ignore(x => x.Command);
    }
}
