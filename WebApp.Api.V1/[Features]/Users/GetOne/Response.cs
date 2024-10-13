using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Users.GetOne;

public sealed record Response
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public UserId? Id { get; init; }
    public string? Email { get; init; }
    public byte[]? Salt { get; init; }
    public byte[]? PasswordHash { get; init; }
    public bool? IsVerified { get; init; }

    public UserProfile? Profile { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    [MapperIgnoreSource(nameof(User.Teams))]
    [MapperIgnoreSource(nameof(User.GoogleAuth))]
    [MapperIgnoreSource(nameof(User.Roles))]
    public static partial Response ToResponse(this User user);
}
