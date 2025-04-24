using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.CreateUploadUrl;

public sealed record CreateWorkspaceResourceUploadUrl
    : ICommand<OneOf<NotFoundError, ServerError, CreateWorkspaceResourceUploadUrlResult>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required string Key { get; init; }
}
