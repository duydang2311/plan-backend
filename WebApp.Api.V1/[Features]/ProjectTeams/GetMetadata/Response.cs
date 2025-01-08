using Riok.Mapperly.Abstractions;
using WebApp.Features.ProjectTeams.GetMetadata;

namespace WebApp.Api.V1.ProjectTeams.GetMetadata;

public sealed record Response
{
    public int Count { get; init; }
    public int TotalCount { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this GetProjectTeamMetadataResult result);
}
