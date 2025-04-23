using System.Text.Json.Serialization;
using NodaTime;
using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DocumentResource), (byte)ResourceType.Document)]
[JsonDerivedType(typeof(FileResource), (byte)ResourceType.File)]
public abstract record Resource
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ResourceId Id { get; init; }
    public ResourceType Type { get; init; }
    public UserId CreatorId { get; init; }
    public User Creator { get; init; } = null!;
}
