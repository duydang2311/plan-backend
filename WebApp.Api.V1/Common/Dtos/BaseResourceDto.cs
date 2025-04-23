using System.Text.Json.Serialization;
using NodaTime;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(BaseDocumentResourceDto), (byte)ResourceType.Document)]
[JsonDerivedType(typeof(BaseFileResourceDto), (byte)ResourceType.File)]
public abstract record BaseResourceDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public ResourceId? Id { get; init; }
    public ResourceType? Type { get; init; }
    public UserId? CreatorId { get; init; }
    public BaseUserDto? Creator { get; init; }
}
