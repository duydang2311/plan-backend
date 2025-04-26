using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.Create;

public sealed record CreateWorkspaceResource : ICommand<OneOf<WorkspaceNotFoundError, UserNotFoundError, Success>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required UserId CreatorId { get; init; }
    public required string Name { get; init; }
    public string? Content { get; init; }
    public ICollection<CreateWorkspaceResourceFile>? Files { get; init; }
}

public sealed record CreateWorkspaceResourceFile
{
    public required string Key { get; init; }
    public required string OriginalName { get; init; }
    public StoragePendingUploadId? PendingUploadId { get; init; }
}
