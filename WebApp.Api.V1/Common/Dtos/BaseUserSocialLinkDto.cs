namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseUserSocialLinkDto
{
    public long? Id { get; init; }
    public string? Url { get; init; }
}
