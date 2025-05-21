using WebApp.Common.Models;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseUserProfileDto
{
    public string? Name { get; init; }
    public string? DisplayName { get; init; }
    public Asset? Image { get; init; }
    public string? Bio { get; init; }
    public ICollection<BaseUserSocialLinkDto>? SocialLinks { get; init; }
}
