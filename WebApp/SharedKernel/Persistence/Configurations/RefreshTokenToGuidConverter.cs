using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class RefreshTokenToGuidConverter : ValueConverter<RefreshToken, Guid>
{
    public RefreshTokenToGuidConverter()
        : base(value => value.Value, (value) => new RefreshToken(value)) { }
}
