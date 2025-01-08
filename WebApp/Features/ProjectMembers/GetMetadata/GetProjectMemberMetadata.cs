using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMembers.GetMetadata;

public sealed record GetProjectMemberMetadata : ICommand<GetProjectMemberMetadataResult>
{
    public ProjectId? ProjectId { get; init; }
    public bool IncludeTotalCount { get; init; }
}
