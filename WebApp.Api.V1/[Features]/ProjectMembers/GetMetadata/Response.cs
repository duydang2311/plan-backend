using Riok.Mapperly.Abstractions;
using WebApp.Features.ProjectMembers.GetMetadata;

namespace WebApp.Api.V1.ProjectMembers.GetMetadata;

public sealed record Response
{
    public int Count { get; init; }
    public int TotalCount { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this GetProjectMemberMetadataResult result);
}
