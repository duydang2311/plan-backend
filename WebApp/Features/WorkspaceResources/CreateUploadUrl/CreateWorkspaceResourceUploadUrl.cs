using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.Common;

namespace WebApp.Features.WorkspaceResources.CreateUploadUrl;

public sealed record CreateWorkspaceResourceUploadUrl
    : ICommand<OneOf<NotFoundError, InvalidResourceTypeError, ServerError, CreateWorkspaceResourceUploadUrlResult>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required ResourceType ResourceType { get; init; }
    public required string Key { get; init; }
}
