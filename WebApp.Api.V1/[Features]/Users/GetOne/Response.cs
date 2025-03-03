using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
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

    public ResponseProfile? Profile { get; init; }

    public sealed record class ResponseProfile
    {
        public UserId UserId { get; init; } = UserId.Empty;
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public Asset Image { get; init; } = Asset.Empty;
        public string? Bio { get; init; }
        public ICollection<UserSocialLink>? SocialLinks { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this User user);

    public static partial Response.ResponseProfile ToResponseProfile(this UserProfile userProfile);
}
