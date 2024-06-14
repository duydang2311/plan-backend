using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class JobRecordConfiguration : IEntityTypeConfiguration<JobRecord>
{
    public void Configure(EntityTypeBuilder<JobRecord> builder)
    {
        builder.ToTable("job_records");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.QueueID);
        builder.Property(x => x.CommandJson).HasColumnType("jsonb");
        builder.Property(x => x.ExecuteAfter);
        builder.Property(x => x.ExpireOn);
        builder.Property(x => x.IsComplete);

        builder.HasKey(x => x.Id);
        builder.Ignore(x => x.Command);
    }
}
