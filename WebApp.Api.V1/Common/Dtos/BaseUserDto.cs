using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseUserDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public UserId? Id { get; init; }
    public string? Email { get; init; }
    public bool? IsVerified { get; init; }
    public BaseUserProfileDto? Profile { get; init; }
}
