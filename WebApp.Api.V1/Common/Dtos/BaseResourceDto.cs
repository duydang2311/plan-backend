using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public record BaseResourceDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public ResourceId? Id { get; init; }
    public UserId? CreatorId { get; init; }
    public BaseUserDto? Creator { get; init; }
    public string? Name { get; init; }
    public string? Rank { get; init; }
    public BaseResourceDocumentDto? Document { get; init; }
    public ICollection<BaseResourceFileDto>? Files { get; init; }

    public int? PreviewFileCount { get; init; }
    public IReadOnlyCollection<string>? PreviewFileMimeTypes { get; init; }
}
