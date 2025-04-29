using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseResourceFileDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public ResourceFileId? Id { get; init; }
    public string? Key { get; init; }
    public string? OriginalName { get; init; }
    public long? Size { get; init; }
    public string? MimeType { get; init; }
}
