using System.Text.Json.Serialization;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Api.V1.UserProfiles.ImageUploaded;

public sealed record Request
{
    public UserId UserId { get; init; }

    [JsonPropertyName("public_id")]
    public string PublicId { get; init; } = null!;

    [JsonPropertyName("resource_type")]
    public string ResourceType { get; init; } = null!;

    public string Format { get; init; } = null!;
    public int Version { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial UserProfileImageUploaded ToEvent(this Request request);
}
