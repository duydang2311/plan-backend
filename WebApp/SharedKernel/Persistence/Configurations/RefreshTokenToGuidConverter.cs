using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class RefreshTokenToGuidConverter : ValueConverter<RefreshToken, Guid>
{
    public RefreshTokenToGuidConverter()
        : base(value => value.Value, (value) => new RefreshToken(value)) { }
}
