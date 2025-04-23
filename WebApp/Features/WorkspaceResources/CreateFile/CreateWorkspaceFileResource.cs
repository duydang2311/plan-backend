using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.Common;

namespace WebApp.Features.WorkspaceResources.CreateFile;

public sealed record CreateWorkspaceFileResource
    : ICommand<OneOf<WorkspaceNotFoundError, UserNotFoundError, InvalidResourceTypeError, Success>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required UserId CreatorId { get; init; }
    public required string Key { get; init; }

    public StoragePendingUploadId? PendingUploadId { get; init; }
}
