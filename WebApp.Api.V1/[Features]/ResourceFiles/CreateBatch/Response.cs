using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ResourceFiles.CreateBatch;

public sealed record Response
{
    public required IReadOnlyCollection<ResourceFileId> Ids { get; init; }
}

public static partial class ResponseMapper
{
    public static Response ToResponse(this IReadOnlyCollection<ResourceFileId> ids) => new() { Ids = ids };
}
