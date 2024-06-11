using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasMaxLength(21)
            .IsFixedLength()
            .HasConversion<UserIdToStringConverter>();
    }
}

public sealed class UserIdToStringConverter : ValueConverter<UserId, string>
{
    public UserIdToStringConverter()
        : base(value => value.Value, (value) => new UserId(value)) { }
}
